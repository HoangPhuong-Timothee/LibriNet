import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-order-filter-dialog',
  templateUrl: './order-filter-dialog.component.html',
  styleUrls: ['./order-filter-dialog.component.css']
})
export class OrderFilterDialogComponent {

  selectedOrderStatus: string = this.data.selectedOrderStatus
  selectedStartDate: Date = this.data.selectedStartDate
  selectedEndDate: Date = this.data.selectedEndDate
  selectedOrderEmail: string = this.data.selectedOrderEmail
  orderStatus = [
    { name: 'Tất cả', value: '' },
    { name: 'Đã giao hàng', value: 'PaymentReceived' },
    { name: 'Đang chờ xử lý', value: 'Pending' },
    { name: 'Đơn hàng đã hủy', value: 'PaymentFailed' }
  ]

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private filterDialogRef: MatDialogRef<OrderFilterDialogComponent>,
    private toastr: ToastrService
  ) { }

  applyFilters() {
    if (this.selectedStartDate && this.selectedEndDate && this.selectedStartDate >= this.selectedEndDate) {
      this.toastr.error("Dữ liệu lọc theo ngày không hợp lệ")
      return
    }
    this.filterDialogRef.close({
      selectedOrderStatus: this.selectedOrderStatus,
      selectedStartDate: this.selectedStartDate,
      selectedEndDate: this.selectedEndDate,
      selectedOrderEmail: this.selectedOrderEmail
    })
  }
}
