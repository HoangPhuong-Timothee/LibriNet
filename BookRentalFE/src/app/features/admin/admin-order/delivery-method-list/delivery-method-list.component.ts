import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { DeliveryMethod } from 'src/app/core/models/delivery-method.model';
import { DeliveryMethodParams } from 'src/app/core/models/params.model';
import { DialogService } from 'src/app/core/services/dialog.service';
import { OrderService } from 'src/app/core/services/order.service';
import { ImportDeliveryMethodFormComponent } from './import-delivery-method-form/import-delivery-method-form.component';

@Component({
  selector: 'app-delivery-method-list',
  templateUrl: './delivery-method-list.component.html',
  styleUrls: ['./delivery-method-list.component.css']
})
export class DeliveryMethodListComponent implements OnInit {

  searchTerm: string = ''
  dmList: DeliveryMethod[] = []
  adminDmParams: DeliveryMethodParams
  totalDms: number = 0
  columns = [
    { field: 'id', header: 'Mã phương thức' },
    { field: 'shortName', header: 'Tên phương thức' },
    { field: 'description', header: 'Mô tả' },
    { field: 'deliveryTime', header: 'Thời gian' },
    { field: 'price', header: 'Giá' },
    {
      field: 'createInfo',
      header: 'Tạo bởi',
      class: () => {
        return 'fst-italic'
      }
    },
    {
      field: 'updateInfo',
      header: 'Cập nhật bởi',
      class: () => {
        return 'fst-italic'
      }
    }
  ]
  actions = [
    {
      label: 'Xóa',
      icon: 'delete',
      tooltip: 'Xóa phương thức',
      action: (row: any) => {
        this.openDeleteDmDialog(row)
      }
    }
  ]

  constructor(
    private orderService: OrderService,
    private dialogService: DialogService,
    private toastr: ToastrService,
    private dialog: MatDialog
  ) { this.adminDmParams = orderService.getAdminDmParams() }

  ngOnInit(): void {
    this.adminDmParams.search = ''
    this.getAllDeliveryMethodsForAdmin()
  }

  getAllDeliveryMethodsForAdmin() {
    this.orderService.getAllDeliveryMethodsForAdmin().subscribe({
      next: (response) => {
        this.dmList = response.data
        this.totalDms = response.count
      },
      error: (error) => {
        console.log("Có lỗi: ", error)
      }
    })
  }

  openImportDmsDialog(): void {
    const dialog = this.dialog.open(ImportDeliveryMethodFormComponent, {
        minWidth: '200px',
        data: {
            title: 'Nhập dữ liệu phương thức giao hàng'
        }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.importSuccess) {
          const params = this.orderService.getAdminDmParams()
          params.pageIndex = 1
          this.orderService.setAdminDmParams(params)
          this.adminDmParams = params
          this.getAllDeliveryMethodsForAdmin()
        }
      }
    })
  }

  async openDeleteDmDialog(dm: DeliveryMethod): Promise<void> {
      const confirmed = await this.dialogService.confirmDialog(
        'XÁC NHẬN XÓA',
        `Bạn chắc chắn muốn xóa phương thức "${dm.shortName}"?`
      )
      if (confirmed) {
        this.orderService.deleteDm(dm.id).subscribe({
          next: async () => {
            this.dmList = this.dmList.filter(g => g.id !== dm.id)
            this.toastr.success(`Xóa phương thức "${dm.shortName}" thành công`)
          },
          error: error => {
            console.log("Có lỗi xảy ra: ", error)
          }
        })
      }
    }

  onPageChange(event: PageEvent) {
      const params = this.orderService.getAdminDmParams()
      params.pageIndex = event.pageIndex + 1
      params.pageSize = event.pageSize
      this.orderService.setAdminDmParams(params)
      this.adminDmParams = params
      this.getAllDeliveryMethodsForAdmin()
    }

    onSearch() {
      const params = this.orderService.getAdminDmParams()
      params.search = this.searchTerm
      params.pageIndex = 1
      this.orderService.setAdminDmParams(params)
      this.adminDmParams = params
      this.getAllDeliveryMethodsForAdmin()
    }

    onReset() {
      if (this.searchTerm) {
        this.searchTerm = ''
      }
      this.adminDmParams = new DeliveryMethodParams()
      this.orderService.setAdminDmParams(this.adminDmParams)
      this.getAllDeliveryMethodsForAdmin()
    }
}
