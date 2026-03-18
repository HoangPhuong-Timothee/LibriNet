import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { InventoryReceipt } from 'src/app/core/models/inventory.model';
import { ReceiptParams } from 'src/app/core/models/params.model';
import { DialogService } from 'src/app/core/services/dialog.service';
import { InventoryService } from 'src/app/core/services/inventory.service';
import { AddExportReceiptFileFormComponent } from './add-export-receipt-file-form/add-export-receipt-file-form.component';
import { AddExportReceiptFormComponent } from './add-export-receipt-form/add-export-receipt-form.component';
import { AddImportReceiptFileFormComponent } from './add-import-receipt-file-form/add-import-receipt-file-form.component';
import { AddImportReceiptFormComponent } from './add-import-receipt-form/add-import-receipt-form.component';
import { ReceiptFilterDialogComponent } from './receipt-filter-dialog/receipt-filter-dialog.component';

@Component({
  selector: 'app-inventory-receipt-list',
  templateUrl: './inventory-receipt-list.component.html',
  styleUrls: ['./inventory-receipt-list.component.css']
})
export class InventoryReceiptListComponent implements OnInit {

  searchTerm: string = ''
  receiptList: InventoryReceipt[] = []
  receiptParams: ReceiptParams
  totalReceipts: number = 0
  columns = [
    { field: 'receiptCode', header: 'Mã phiếu' },
    {
      field: 'receiptType',
      header: 'Loại phiếu',
      class: (row: InventoryReceipt) => {
        if (row.receiptType === 'Nhập kho')
          return 'text-success fw-bold'
        if (row.receiptType === 'Xuất kho')
          return 'text-danger fw-bold'
        return ''
      }
    },
    { field: 'totalAmount', header: 'Tổng số lượng' },
    { field: 'totalPrice', header: 'Tồng phí' },
    { field: 'importNotes', header: 'Ghi chú' },
    {
      field: 'receiptStatus',
      header: 'Trạng thái',
      class: (row: InventoryReceipt) => {
        if (row.receiptStatus === 'Đang chờ xử lý')
          return 'rounded text-white p-2 bg-warning fw-bold border border-warning'
        if (row.receiptStatus === 'Hoàn thành')
          return 'rounded text-white p-2 bg-success fw-bold border border-success'
        if (row.receiptStatus === 'Hủy bỏ')
          return 'rounded text-white p-2 bg-danger fw-bold border border-danger'
        return ''
      }
    },
    {
      field: 'createInfo',
      header: 'Tạo bởi',
      class: () => 'fst-italic'
    },
    {
      field: 'updateInfo',
      header: 'Cập nhật bởi',
      class: () => 'fst-italic'
  }
  ]
  actions = [
   {
      label: 'Xem chi tiết',
      icon: 'visibility',
      tooltip: 'Xem thông tin chi tiết phiếu',
      action: (row: InventoryReceipt) => {
        this.openInventoryReceiptDetailsDialog(row)
      }
    },
    {
      label: 'Chấp nhận phiếu',
      icon: 'check_circle',
      tooltip: 'Xác nhận xuất - nhập kho',
      action: (row:InventoryReceipt) => {
        this.acceptReceipt(row)
      }
    },
    {
      label: 'Hủy bỏ phiếu',
      icon: 'cancel',
      tooltip: 'Hủy bỏ xuất - nhập kho',
      action: (row: InventoryReceipt) => {
        this.cancelReceipt(row)
      }
    }
  ]

  constructor(
    private inventoryService: InventoryService,
    private toastr: ToastrService,
    private dialog: MatDialog,
    private dialogService: DialogService
  ) { this.receiptParams = inventoryService.getInventoryReceiptParams() }

  ngOnInit(): void {
    this.searchTerm = ''
    this.getAllInventoryReceipts()
  }

  getAllInventoryReceipts() {
    this.inventoryService.getAllInventoryReceipts().subscribe({
      next: (response) => {
        this.receiptList = response.data
        this.totalReceipts = response.count
      },
      error: error => {
        this.toastr.error(error.message)
        console.log("Có lỗi: ", error)
      }
    })
  }

  openAddImportReceiptFileDialog() {
    const dialog = this.dialog.open(AddImportReceiptFileFormComponent, {
      minWidth: '200px',
      data: {
        title: 'Nhập file phiếu nhập kho'
      }
    });
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.importSucess) {
          const params = this.inventoryService.getInventoryReceiptParams()
          params.pageIndex = 1
          this.inventoryService.setInventoryReceiptParams(params)
          this.receiptParams = params
          this.getAllInventoryReceipts()
        }
      }
    })
  }

  openAddExportReceiptFileDialog() {
    const dialog = this.dialog.open(AddExportReceiptFileFormComponent, {
      minWidth: '200px',
      data: {
        title: 'Nhập file phiếu xuất kho'
      }
    });
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.exportSuccess) {
          const params = this.inventoryService.getInventoryReceiptParams()
          params.pageIndex = 1
          this.inventoryService.setInventoryReceiptParams(params)
          this.receiptParams = params
          this.getAllInventoryReceipts()
        }
      }
    })
  }

  openAddImportReceiptFormDialog() {
    const dialog = this.dialog.open(AddImportReceiptFormComponent, {
      minWidth: '1500px',
      data: {
        title: 'Tạo phiếu nhập kho'
      }
    });
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.importSucess) {
          const params = this.inventoryService.getInventoryReceiptParams()
          params.pageIndex = 1
          this.inventoryService.setInventoryReceiptParams(params)
          this.receiptParams = params
          this.getAllInventoryReceipts()
        }
      }
    })
  }

  openAddExportReceiptFormDialog() {
    const dialog = this.dialog.open(AddExportReceiptFormComponent, {
      minWidth: '1500px',
      data: {
        title: 'Tạo phiếu xuất kho'
      }
    });
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.exportSuccess) {
          const params = this.inventoryService.getInventoryReceiptParams()
          params.pageIndex = 1
          this.inventoryService.setInventoryReceiptParams(params)
          this.receiptParams = params
          this.getAllInventoryReceipts()
        }
      }
    })
  }

  openFilterDialog() {
    const dialog = this.dialog.open(ReceiptFilterDialogComponent, {
      minWidth: '300px',
      data: {
        title: 'Lọc dữ liệu',
        selectedReceiptStatus: this.receiptParams.receiptStatus,
        selectedReceiptType: this.receiptParams.receiptType,
        selectedStartDate: this.receiptParams.startDate,
        selectedEndDate: this.receiptParams.endDate
      }
    })
    dialog.afterClosed().subscribe({
      next: result => {
        if (result) {
          const params = this.inventoryService.getInventoryReceiptParams()
          params.pageIndex = 1
          params.receiptStatus = result.selectedReceiptStatus
          params.receiptType = result.selectedReceiptType
          params.startDate = result.selectedStartDate
          params.endDate = result.selectedEndDate
          this.inventoryService.setInventoryReceiptParams(params)
          this.receiptParams = params
          this.getAllInventoryReceipts()
        }
      }
    })
  }

  openInventoryReceiptDetailsDialog(receipt: InventoryReceipt) {

  }

  async acceptReceipt(receipt: InventoryReceipt) {
    const confirmed = await this.dialogService.confirmDialog(
      'XÁC NHẬN TRẠNG THÁI PHIẾU',
      `Bạn chắc chắn muốn xác nhận chấp nhận và hoàn thành mã phiếu "${receipt.receiptCode}"?`
    )
    if (confirmed) {
      this.inventoryService.acceptReceipt(receipt.receiptId, receipt.receiptType).subscribe({
        next: (response) => {
          this.toastr.success(response.message)
          this.getAllInventoryReceipts()
        },
        error: (error) => {
          console.error('Có lỗi:', error)
          this.toastr.error(error.message)
        }
      })
    }
  }

  async cancelReceipt(receipt: InventoryReceipt) {
    const confirmed = await this.dialogService.confirmDialog(
      'XÁC NHẬN TRẠNG THÁI PHIẾU',
      `Bạn chắc chắn muốn hủy bỏ mã phiếu "${receipt.receiptCode}"?`
    )
    if (confirmed) {
      this.inventoryService.cancelReceipt(receipt.receiptId, receipt.receiptType).subscribe({
        next: (response) => {
          this.toastr.success(response.message)
          this.getAllInventoryReceipts()
        },
        error: (error) => {
          console.error('Có lỗi:', error)
          this.toastr.error(error.message)
        }
      })
    }
  }

  onSearch() {
    const params = this.inventoryService.getInventoryReceiptParams()
    params.pageIndex = 1
    params.search = this.searchTerm
    this.inventoryService.setInventoryReceiptParams(params)
    this.receiptParams = params
    this.getAllInventoryReceipts()
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm = ''
    }
    this.receiptParams = new ReceiptParams()
    this.inventoryService.setInventoryReceiptParams(this.receiptParams)
    this.getAllInventoryReceipts()
  }

  onPageChange(event: PageEvent) {
    const params = this.inventoryService.getInventoryReceiptParams()
    params.pageIndex = event.pageIndex + 1
    params.pageSize = event.pageSize
    this.inventoryService.setInventoryReceiptParams(params)
    this.receiptParams = params
    this.getAllInventoryReceipts()
  }
}
