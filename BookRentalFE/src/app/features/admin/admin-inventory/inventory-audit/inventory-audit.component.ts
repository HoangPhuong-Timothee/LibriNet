import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { InventoryAudit, InventoryAuditDetails } from 'src/app/core/models/inventory.model';
import { InventoryAuditParams } from 'src/app/core/models/params.model';
import { DialogService } from 'src/app/core/services/dialog.service';
import { InventoryService } from 'src/app/core/services/inventory.service';
import { AddInventoryAuditFormComponent } from './add-inventory-audit-form/add-inventory-audit-form.component';
import { ConductInventoryFormComponent } from './conduct-inventory-form/conduct-inventory-form.component';
import { FilterInventoryAuditDialogComponent } from './filter-inventory-audit-dialog/filter-inventory-audit-dialog.component';
import { InventoryAuditResultComponent } from './inventory-audit-result/inventory-audit-result.component';

@Component({
  selector: 'app-inventory-audit',
  templateUrl: './inventory-audit.component.html',
  styleUrls: ['./inventory-audit.component.css']
})
export class InventoryAuditComponent implements OnInit {

  searchTerm: string = ''
  invAuditList: InventoryAudit[] = []
  invAuditDetails: InventoryAuditDetails[] = []
  invAuditParams: InventoryAuditParams
  totalInvAudit: number = 0
  columns = [
    { field: 'auditCode', header: 'Mã kiểm kê' },
    {
      field: 'auditDate',
      header: 'Ngày kiểm kê',
      class: () => 'fst-italic'
    },
    { field: 'audittedBy', header: 'Nhân viên kiểm kê' },
    { field: 'auditNotes', header: 'Nội dung' },
    {
      field: 'auditStatus',
      header: 'Tình trạng kiểm kê',
      class: (row: InventoryAudit) => {
        if (row.auditStatus === 'Chưa thực hiện')
          return 'rounded text-white p-2 bg-warning fw-bold border border-warning'
        if (row.auditStatus === 'Đã thực hiện')
          return 'rounded text-white p-2 bg-success fw-bold border border-success'
        if (row.auditStatus === 'Đang thực hiện')
          return 'rounded text-white p-2 bg-primary fw-bold border border-danger'
        return ''
      }
    }
  ]
  actions = [
    {
      label: 'Xem kết quả kiểm kê',
      icon: 'visibility',
      tooltip: 'Xem thông tin chi tiết kết quả kiểm kê hàng hóa',
      action: (row: InventoryAudit) => {
        this.openInventoryAuditResultDialog(row)
      },
      disabled: (row: InventoryAudit) => {
        return row.auditStatus === 'Chưa thực hiện'
      }
    },
    {
      label: 'Thực hiện kiểm kê',
      icon: 'calculate',
      tooltip: 'Thực hiện kiểm kê',
      action: (row: InventoryAudit) => {
        this.openConductInventoryDialog(row)
      },
      disabled: (row: InventoryAudit) => {
        return row.auditStatus === 'Đã thực hiện' || row.auditStatus === 'Đang thực hiện'
      }
    },
    {
      label: 'Xóa',
      icon: 'delete',
      tooltip: 'Xóa kế hoạch kiểm kê',
      action: (row: InventoryAudit) => {
        this.openDeleteInventoryAuditDialog(row)
      }
    }
  ]

  constructor(
    private inventoryService: InventoryService,
    private dialog: MatDialog,
    private toastr: ToastrService,
    private dialogService: DialogService
  ) { this.invAuditParams = this.inventoryService.getInventoryAuditParams() }

  ngOnInit(): void {
    this.invAuditParams.search = ''
    this.getAllInventoryAudits()
  }

  getAllInventoryAudits() {
    this.inventoryService.getAllInventoryAudits().subscribe({
      next: (response) => {
        this.invAuditList = response.data
        this.totalInvAudit = response.count
      },
      error: (error) => {
        console.log("Có lỗi: ", error)
      }
    })
  }

  openFilterInventoryAudit() {
    const dialog = this.dialog.open(FilterInventoryAuditDialogComponent, {
      minWidth: '500px',
      data: {
        title: 'Lọc dữ liệu',
        selectedAuditStatus: this.invAuditParams.auditStatus,
        selectedStartDate: this.invAuditParams.startDate,
        selectedEndDate: this.invAuditParams.endDate,
        selectedAudittedBy: this.invAuditParams.audittedBy
      }
    })
    dialog.afterClosed().subscribe({
      next: result => {
        if (result) {
          const params = this.inventoryService.getInventoryAuditParams()
          params.auditStatus = result.selectedAuditStatus
          params.startDate = result.selectedStartDate
          params.endDate = result.selectedEndDate
          params.audittedBy = result.selectedAudittedBy
          params.pageIndex = 1
          this.inventoryService.setInventoryAuditParams(params)
          this.invAuditParams = params
          this.getAllInventoryAudits()
        }
      }
    })
  }

  openAddNewInventoryAuditDialog() {
    const dialog = this.dialog.open(AddInventoryAuditFormComponent, {
      minWidth: '1500px',
      data: {
        title: 'Tạo kế hoạch kiểm kê hàng hóa'
      }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.success) {
          const params = this.inventoryService.getInventoryAuditParams()
          params.pageIndex = 1
          this.inventoryService.setInventoryAuditParams(params)
          this.invAuditParams = params
          this.getAllInventoryAudits()
        }
      },
      error: (error) => {
        this.toastr.error(error.message)
        console.log("Có lỗi: ", error)
      }
    })
  }

  openConductInventoryDialog(invAudit: InventoryAudit) {
    this.inventoryService.getInventoryAuditDetails(invAudit.id).subscribe({
      next: (response) => {
        this.invAuditDetails = response
        const dialog = this.dialog.open(ConductInventoryFormComponent, {
          minWidth: '1500px',
          data: {
            title: `Thực hiện kiểm kê hàng hóa mã '${invAudit.auditCode}'`,
            invAudit: invAudit,
            details: this.invAuditDetails
          }
        })
        dialog.afterClosed().subscribe({
          next: (result) => {
            if (result && result.success) {
              const params = this.inventoryService.getInventoryAuditParams()
              params.pageIndex = 1
              this.inventoryService.setInventoryAuditParams(params)
              this.invAuditParams = params
              this.getAllInventoryAudits()
            }
          },
          error: (error) => {
            this.toastr.error(error.message)
            console.log("Có lỗi: ", error)
          }
        })
      },
      error: (error) => {
        this.toastr.error(error.message)
        console.log("Có lỗi: ", error)
      }
    })
  }

  openInventoryAuditResultDialog(invAudit: InventoryAudit) {
    const dialog = this.dialog.open(InventoryAuditResultComponent, {
      minWidth: '1500px',
      data: {
        title: 'Kết quả kiểm kê'
      }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {

      },
      error: (error) => {
        this.toastr.error(error.message)
        console.log("Có lỗi: ", error)
      }
    })
  }

  async openDeleteInventoryAuditDialog(invAudit: InventoryAudit): Promise<void> {
    const confirmed = await this.dialogService.confirmDialog(
      'XÁC NHẬN XÓA',
      `Bạn chắc chắn muốn xóa kế hoạch "${invAudit.auditCode}"?`
    )
    if (confirmed) {
      this.inventoryService.deleteInventoryAudit(invAudit.id).subscribe({
        next: async () => {
          this.invAuditList = this.invAuditList.filter(ia => ia.id !== invAudit.id)
          this.toastr.success(`Xóa kế hoạch kiểm kê mã: "${invAudit.auditCode}" thành công`)
        },
        error: error => {
          this.toastr.error(error.message)
          console.log("Có lỗi xảy ra: ", error)
        }
      })
    }
  }

  onPageChange(event: PageEvent) {
      const params = this.inventoryService.getInventoryAuditParams()
      params.pageIndex = event.pageIndex + 1
      params.pageSize = event.pageSize
      this.inventoryService.setInventoryAuditParams(params)
      this.invAuditParams = params
      this.getAllInventoryAudits()
    }

  onSearch() {
    const params = this.inventoryService.getInventoryAuditParams()
    params.search = this.searchTerm
    params.pageIndex = 1
    this.inventoryService.setInventoryAuditParams(params)
    this.invAuditParams = params
    this.getAllInventoryAudits()
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm = ''
    }
    this.invAuditParams = new InventoryAuditParams()
    this.inventoryService.setInventoryAuditParams(this.invAuditParams)
    this.getAllInventoryAudits()
  }
}
