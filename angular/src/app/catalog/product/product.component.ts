import { PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';

import { ProductType } from '@proxy/nem-ecommerce/products';
import { ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, take, takeUntil } from 'rxjs';
import { NotificationService } from '../../shared/services/notification.service';
import { ProductAttributeComponent } from './product-attribute.component';
import { ProductDetailComponent } from './product-detail.component';
import { ProductDto, ProductInListDto, ProductsService } from '@proxy/catalog/products';
import { ProductCategoriesService, ProductCategoryInListDto } from '@proxy/catalog/product-categories';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
})
export class ProductComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  items: ProductInListDto[] = [];
  public selectedItems: ProductInListDto[] = [];

  //Paging variables
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;
  
  //Filter
  productCategories: any[] = [];
  keyword: string = '';
  categoryId: string = '';

  constructor(
    private productService: ProductsService,
    private productCategoryService: ProductCategoriesService,
    private dialogService: DialogService,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.loadProductCategories();
    this.loadData();
  }

  loadData(selectionId = null) {
    this.toggleBlockUI(true);
  
    if (!this.keyword) {
      // If keyword is empty, call getListAll
      this.productService
        .getListAll()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: ProductInListDto[]) => {
            this.items = response;
            this.totalCount = response.length; // Since there's no pagination, totalCount is the length of the response array
            if (selectionId != null && this.items.length > 0) {
              this.selectedItems = this.items.filter(x => x.id == selectionId);
            }
            this.toggleBlockUI(false);
          },
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    } else {
      // If keyword is not empty, call getListFilter
      this.productService
        .getListFilter({
          maxResultCount: this.maxResultCount,
          skipCount: this.skipCount,
          keyword: this.keyword,
          categoryId: this.categoryId,
        })
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: PagedResultDto<ProductInListDto>) => {
            this.items = response.items;
            this.totalCount = response.totalCount;
            if (selectionId != null && this.items.length > 0) {
              this.selectedItems = this.items.filter(x => x.id == selectionId);
            }
            this.toggleBlockUI(false);
          },
          error: () => {
            this.toggleBlockUI(false);
          },
        });
    }
  }

  loadProductCategories() {
    this.productCategoryService.getListAll().subscribe((response: ProductCategoryInListDto[]) => {
      response.forEach(element => {
        this.productCategories.push({
          value: element.id,
          label: element.name,
        });
      });
    });
  }

  pageChanged(event: any): void {
    this.skipCount = (event.page - 1) * this.maxResultCount;
    this.maxResultCount = event.rows;
    this.loadData();
  }
  showAddModal() {
    const ref = this.dialogService.open(ProductDetailComponent, {
      header: 'Thêm mới sản phẩm',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
        this.notificationService.showSuccess('Thêm sản phẩm thành công');
        this.selectedItems = [];
      }
    });
  }

  showEditModal() {
    if (this.selectedItems.length == 0) {
      this.notificationService.showError('Bạn phải chọn một bản ghi');
      return;
    }
    const id = this.selectedItems[0].id;
    const ref = this.dialogService.open(ProductDetailComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật sản phẩm',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
        this.selectedItems = [];
        this.notificationService.showSuccess('Cập nhật sản phẩm thành công');
      }
    });
  }
  manageProductAttribute(id: string) {
    const ref = this.dialogService.open(ProductAttributeComponent, {
      data: {
        id: id,
      },
      header: 'Quản lý thuộc tính sản phẩm',
      width: '70%',
      height:'40%',
    });

    ref.onClose.subscribe((data: ProductDto) => {
      if (data) {
        this.loadData();
        this.selectedItems = [];
        this.notificationService.showSuccess('Cập nhật thuộc tính sản phẩm thành công');
      }
    });
  }
  deleteItems() {
    if (this.selectedItems.length == 0) {
      this.notificationService.showError('Phải chọn ít nhất một bản ghi');
      return;
    }
    var ids = [];
    this.selectedItems.forEach(element => {
      ids.push(element.id);
    });
    this.confirmationService.confirm({
      message: 'Bạn có chắc muốn xóa bản ghi này?',
      accept: () => {
        this.deleteItemsConfirmed(ids);
      },
    });
  }

  deleteItemsConfirmed(ids: string[]) {
    this.toggleBlockUI(true);
    this.productService
      .deleteMultiple(ids)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.notificationService.showSuccess('Xóa thành công');
          this.loadData();
          this.selectedItems = [];
          this.toggleBlockUI(false);
        },
        error: () => {
          this.toggleBlockUI(false);
        },
      });
  }

  getProductTypeName(value: number) {
    return ProductType[value];
  }

  private toggleBlockUI(enabled: boolean) {
    if (enabled == true) {
      this.blockedPanel = true;
    } else {
      setTimeout(() => {
        this.blockedPanel = false;
      }, 1000);
    }
  }
}