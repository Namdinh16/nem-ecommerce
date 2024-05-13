import { PagedResultDto } from '@abp/ng.core';
import { Component, OnDestroy, OnInit } from '@angular/core';


import { ConfirmationService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { Subject, take, takeUntil } from 'rxjs';
import { NotificationService } from '../../shared/services/notification.service';



import { ManufactureDetailComponent } from './manufacture-detail.component';
import { ManufacturerDto, ManufacturerInListDto, ManufacturersService } from '@proxy/catalog/manufacturers';


@Component({
  selector: 'app-manufacture',
  templateUrl: './manufacture.component.html',

})
export class ManufactureComponent implements OnInit, OnDestroy {
  private ngUnsubscribe = new Subject<void>();
  blockedPanel: boolean = false;
  items: ManufacturerInListDto[] = [];
  public selectedItems: ManufacturerInListDto[] = [];

  //Paging variables
  public skipCount: number = 0;
  public maxResultCount: number = 10;
  public totalCount: number;

  //Filter

  keyword: string = '';
  // categoryId: string = '';

  constructor(
    private manufactureService: ManufacturersService,
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
      this.manufactureService
        .getListAll()
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: ManufacturerInListDto[]) => {
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
      this.manufactureService
        .getListFilter({
          maxResultCount: this.maxResultCount,
          skipCount: this.skipCount,
          keyword: this.keyword,
        })
        .pipe(takeUntil(this.ngUnsubscribe))
        .subscribe({
          next: (response: PagedResultDto<ManufacturerInListDto>) => {
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
    const ref = this.dialogService.open(ManufactureDetailComponent, {
      header: 'Thêm mới nhà sản xuất',
      width: '70%',
    });

    ref.onClose.subscribe((data: ManufacturerDto) => {
      if (data) {
        this.loadData();
        this.notificationService.showSuccess('Thêm thành công');
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
    const ref = this.dialogService.open(ManufactureDetailComponent, {
      data: {
        id: id,
      },
      header: 'Cập nhật thông tin nhà sản xuất',
      width: '70%',
    });

    ref.onClose.subscribe((data: ManufacturerDto) => {
      if (data) {
        this.loadData();
        this.selectedItems = [];
        this.notificationService.showSuccess('Cập nhật thành công');
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
    this.manufactureService.deleteMultiple(ids).pipe(takeUntil(this.ngUnsubscribe)).subscribe({
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