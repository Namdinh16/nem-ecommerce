import { PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';


import { ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, take, takeUntil } from 'rxjs';
import { NotificationService } from '../../shared/services/notification.service';


import { ProductCategoriesService, ProductCategoryDto, ProductCategoryInListDto } from '@proxy/catalog/product-categories';
import { CategoryDetailComponent } from './category-detail.component';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',

})
export class CategoryComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  items: ProductCategoryInListDto[] = [];
  public selectedItems: ProductCategoryInListDto[] = [];

  //Paging variables
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;

  //Filter

  keyword: string = '';
  categoryId: string = '';

  constructor(
    private categoryService: ProductCategoriesService,
    private dialogService: DialogService,
    private notificationService: NotificationService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  ngOnInit(): void {
    this.loadData();
  }


  loadData(selectionId = null) {
    this.toggleBlockUI(true);
  
    if (!this.keyword) {
      // If keyword is empty, call getListAll
      this.categoryService
        .getListAll()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: ProductCategoryInListDto[]) => {
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
      this.categoryService
        .getListFilter({
          maxResultCount: this.maxResultCount,
          skipCount: this.skipCount,
          keyword: this.keyword,
        })
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: PagedResultDto<ProductCategoryInListDto>) => {
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

  pageChanged(event: any): void {
    this.skipCount = (event.page - 1) * this.maxResultCount;
    this.maxResultCount = event.rows;
    this.loadData();
  }
  showAddModal() {
    const ref = this.dialogService.open(CategoryDetailComponent, {
      header: 'Thêm mới danh mục',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductCategoryDto) => {
      if (data) {
        this.loadData();
        this.notificationService.showSuccess('Thêm danh mục thành công');
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
    const ref = this.dialogService.open(CategoryDetailComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật danh mục',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductCategoryDto) => {
      if (data) {
        this.loadData();
        this.selectedItems = [];
        this.notificationService.showSuccess('Cập nhật danh mục thành công');
      }
    });
  }
  deleteItems(){
    if(this.selectedItems.length == 0){
      this.notificationService.showError("Phải chọn ít nhất một bản ghi");
      return;
    }
    var ids =[];
    this.selectedItems.forEach(element=>{
      ids.push(element.id);
    });
    this.confirmationService.confirm({
      message:'Bạn có chắc muốn xóa bản ghi này?',
      accept:()=>{
        this.deleteItemsConfirmed(ids);
      }
    })
  }

  deleteItemsConfirmed(ids: string[]){
    this.toggleBlockUI(true);
    this.categoryService.deleteMultiple(ids).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
      next: ()=>{
        this.notificationService.showSuccess("Xóa thành công");
        this.loadData();
        this.selectedItems = [];
        this.toggleBlockUI(false);
      },
      error:()=>{
        this.toggleBlockUI(false);
      }
    })
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