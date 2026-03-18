import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-filter-inventory-audit-dialog',
  templateUrl: './filter-inventory-audit-dialog.component.html',
  styleUrls: ['./filter-inventory-audit-dialog.component.css']
})
export class FilterInventoryAuditDialogComponent {

  selectedAuditStatus: string = this.data.selectedAUditStatus
  selectedStartDate: Date = this.data.selectedStartDate
  selectedEndDate: Date = this.data.selectedEndDate
  selectedAudittedBy: string = this.data.selectedAudittedBy
  auditStatus = [
    { name: 'Tất cả', value: '' },
    { name: 'Đã thực hiện', value: 'done' },
    { name: 'Đang thực hiện', value: 'proccessing' },
    { name: 'Chưa thực hiện', value: 'undone' }
  ]

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private filterDialogRef: MatDialogRef<FilterInventoryAuditDialogComponent>,
    private toastr: ToastrService
  ) { }

  applyFilters() {
    if (this.selectedStartDate && this.selectedEndDate && this.selectedStartDate >= this.selectedEndDate) {
      this.toastr.error("Dữ liệu lọc theo ngày không hợp lệ")
      return
    }
    this.filterDialogRef.close({
      selectedAuditStatus: this.selectedAuditStatus,
      selectedStartDate: this.selectedStartDate,
      selectedEndDate: this.selectedEndDate,
      selectedAudittedBy: this.selectedAudittedBy
    })
  }
}
