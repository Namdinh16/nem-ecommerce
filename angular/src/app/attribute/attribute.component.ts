import { PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ProductAttributeDto, ProductAttributeInListDto, ProductAttributesService } from '@proxy/product-attributes';
import { AttributeType } from '@proxy/nem-ecommerce/product-attributes';
import { ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, take, takeUntil } from 'rxjs';
import { NotificationService } from '../shared/services/notification.service';
import { AttributeDetailComponent } from './attribute-detail.component';

@Component({
  selector: 'app-attribute',
  templateUrl: './attribute.component.html',
  styleUrls: ['./attribute.component.scss'],
})
export class AttributeComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  items: ProductAttributeInListDto[] = [];
  public selectedItems: ProductAttributeInListDto[] = [];

  //Paging variables
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;

  //Filter
  AttributeCategories: any[] = [];
  keyword: string = '';
  categoryId: string = '';

  constructor(
    private attributeService: ProductAttributesService,
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

  // loadData() {
  //   this.toggleBlockUI(true);
  //   this.attributeService
  //     .getListFilter({
  //       keyword: this.keyword,
  //       maxResultCount: this.maxResultCount,
  //       skipCount: this.skipCount,
  //     })
  //     .pipe(takeUntil(this.ngUnsubscribe))
  //     .subscribe({
  //       next: (response: PagedResultDto<ProductAttributeInListDto>) => {
  //         this.items = response.items;
  //         this.totalCount = response.totalCount;
  //         this.toggleBlockUI(false);
  //       },
  //       error: () => {
  //         this.toggleBlockUI(false);
  //       },
  //     });
  // }

  // loadData() {
  //   this.toggleBlockUI(true);
  
  //   // Kiểm tra xem keyword có giá trị không
  //   const hasKeyword = this.keyword.trim() !== '';
  
  //   // Tạo yêu cầu API chỉ khi có keyword
  //   const requestParams = hasKeyword ? {
  //     keyword: this.keyword.trim(),
  //     maxResultCount: this.maxResultCount,
  //     skipCount: this.skipCount,
  //   } : {
  //     maxResultCount: this.maxResultCount,
  //     skipCount: this.skipCount,
  //   };
  
  //   // Thực hiện yêu cầu API chỉ khi có keyword
  //   if (hasKeyword) {
  //     this.attributeService
  //       .getListFilter(requestParams)
  //       .pipe(takeUntil(this.ngUnsubscribe))
  //       .subscribe({
  //         next: (response: PagedResultDto<ProductAttributeInListDto>) => {
  //           this.items = response.items;
  //           this.totalCount = response.totalCount;
  //           this.toggleBlockUI(false);
  //         },
  //         error: () => {
  //           this.toggleBlockUI(false);
  //         },
  //       });
  //   } else {
  //     this.toggleBlockUI(false);
  //   }
  // }

  loadData(selectionId = null) {
    this.toggleBlockUI(true);
  
    if (!this.keyword) {
      // If keyword is empty, call getListAll
      this.attributeService
        .getListAll()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: ProductAttributeInListDto[]) => {
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
      this.attributeService
        .getListFilter({
          maxResultCount: this.maxResultCount,
          skipCount: this.skipCount,
          keyword: this.keyword,
        })
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: PagedResultDto<ProductAttributeInListDto>) => {
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
    const ref = this.dialogService.open(AttributeDetailComponent, {
      header: 'Thêm mới sản phẩm',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductAttributeDto) => {
      if (data) {
        this.loadData();
        this.notificationService.showSuccess('Thêm thuộc tính thành công');
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
    const ref = this.dialogService.open(AttributeDetailComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật sản phẩm',
      width: '70%',
    });

    ref.onClose.subscribe((data: ProductAttributeDto) => {
      if (data) {
        this.loadData();
        this.selectedItems = [];
        this.notificationService.showSuccess('Cập nhật thuộc tính thành công');
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
    this.attributeService.deleteMultiple(ids).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
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
  
  getAttributeTypeName(value: number){
    return AttributeType[value];
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