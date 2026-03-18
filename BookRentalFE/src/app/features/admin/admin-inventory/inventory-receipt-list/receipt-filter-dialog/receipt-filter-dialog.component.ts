import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-receipt-filter-dialog',
  templateUrl: './receipt-filter-dialog.component.html',
  styleUrls: ['./receipt-filter-dialog.component.css']
})
export class ReceiptFilterDialogComponent {

  selectedReceiptStatus: string = this.data.selectedReceiptStatus
  selectedStartDate: Date = this.data.selectedStartDate
  selectedEndDate: Date = this.data.selectedEndDate
  selectedReceiptType: string = this.data.selectedReceiptType
  receiptStatusOptions = [
    { name: 'Tất cả', value: '' },
    { name: 'Đang chờ xử lý', value: 'Pending' },
    { name: 'Hoàn thành', value: 'Accept' },
    { name: 'Hủy bỏ', value: 'Cancel' }
  ]
  receiptTypeOptions = [
    { name: 'Tất cả', value: '' },
    { name: 'Phiếu nhập', value: 'import' },
    { name: 'Phiếu xuất', value: 'export' },
  ]

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private filterDialogRef: MatDialogRef<ReceiptFilterDialogComponent>,
    private toastr: ToastrService
  ) { }

  applyFilters() {
    if (this.selectedStartDate && this.selectedEndDate && this.selectedStartDate >= this.selectedEndDate) {
      this.toastr.error("Dữ liệu lọc theo ngày không hợp lệ")
      return
    }
    this.filterDialogRef.close({
      selectedReceiptStatus: this.selectedReceiptStatus,
      selectedStartDate: this.selectedStartDate,
      selectedEndDate: this.selectedEndDate,
      selectedReceiptType: this.selectedReceiptType
    })
  }
}
