import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { UnitOfMeasureParams } from 'src/app/core/models/params.model';
import { UnitOfMeasure } from 'src/app/core/models/unit-of-measure.model';
import { DialogService } from 'src/app/core/services/dialog.service';
import { UnitOfMeasureService } from 'src/app/core/services/unit-of-measure.service';
import { AddUnitOfMeasureFormComponent } from './add-unit-of-measure-form/add-unit-of-measure-form.component';
import { EditUnitOfMeasureFormComponent } from './edit-unit-of-measure-form/edit-unit-of-measure-form.component';
import { ImportMeasureUnitFormComponent } from './import-measure-unit-form/import-measure-unit-form.component';

@Component({
  selector: 'app-unit-of-measure-list',
  templateUrl: './unit-of-measure-list.component.html',
  styleUrls: ['./unit-of-measure-list.component.css']
})
export class UnitOfMeasureListComponent implements OnInit {

  searchTerm: string = ''
  unitOfMeasuresList: UnitOfMeasure[] = []
  adminUnitOfMeasureParams: UnitOfMeasureParams
  totalUnitOfMeasures = 0
  columns = [
    { field: 'id', header: 'Mã đơn vị' },
    { field: 'name', header: 'Tên' },
    { field: 'description', header: 'Mô tả' },
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
      label: 'Cập nhật',
      icon: 'edit',
      tooltip: 'Chỉnh sửa đơn vị đo',
      action: (row: any) => {
        this.openUpdateUnitOfMeasureDialog(row)
      }
    },
    {
      label: 'Xóa',
      icon: 'delete',
      tooltip: 'Xóa đơn vị đo',
      action: (row: any) => {
        this.openDeleteUnitOfMeasureDialog(row)
      }
    }
  ]

  constructor(
    private uomService: UnitOfMeasureService,
    private toastr: ToastrService,
    private dialog: MatDialog,
    private dialogService: DialogService
  ) { this.adminUnitOfMeasureParams = uomService.getUnitOfMeasureParams() }

  ngOnInit(): void {
    this.adminUnitOfMeasureParams.search = ''
    this.getAllUnitOfMeasuresForAdmin()
  }

  getAllUnitOfMeasuresForAdmin() {
    this.uomService.getUnitOfMeasuresForAdmin().subscribe({
      next: response => {
        this.unitOfMeasuresList = response.data
        this.totalUnitOfMeasures = response.count
      },
      error: error => {
        console.log("Có lỗi: ", error)
      }
    })
  }

  openAddNewUnitOfMeasureDialog() {
    const dialog = this.dialog.open(AddUnitOfMeasureFormComponent, {
        minWidth: '500px',
        data: {
            title: 'Thêm đơn vị'
        }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.success) {
          const params = this.uomService.getUnitOfMeasureParams()
          params.pageIndex = 1
          this.uomService.setUnitOfMeasureParams(params)
          this.adminUnitOfMeasureParams = params
          this.getAllUnitOfMeasuresForAdmin()
        }
      }
    })
  }

  openImportUnitOfMeasureDialog() {
    const dialog = this.dialog.open(ImportMeasureUnitFormComponent, {
        minWidth: '200px',
        data: {
            title: 'Nhập dữ liệu đơn vị'
        }
    })
    dialog.afterClosed().subscribe({
      next: (result) => {
        if (result && result.importSuccess) {
          const params = this.uomService.getUnitOfMeasureParams()
          params.pageIndex = 1
          this.uomService.setUnitOfMeasureParams(params)
          this.adminUnitOfMeasureParams = params
          this.getAllUnitOfMeasuresForAdmin()
        }
      }
    })
  }

  openUpdateUnitOfMeasureDialog(unitOfMeasure: UnitOfMeasure) {
    const dialog = this.dialog.open(EditUnitOfMeasureFormComponent, {
      minWidth: '500px',
      data: {
        title: `Chỉnh sửa đơn vị '${unitOfMeasure.name}'`,
        unitOfMeasure
      }
    })
    dialog.afterClosed().subscribe({
      next: result => {
        if (result) {
          if (result && result.success) {
            const params = this.uomService.getUnitOfMeasureParams()
            params.pageIndex = 1
            this.uomService.setUnitOfMeasureParams(params)
            this.adminUnitOfMeasureParams = params
            this.getAllUnitOfMeasuresForAdmin()
          }
        }
      }
    })
  }

  async openDeleteUnitOfMeasureDialog(unitOfMeasure: UnitOfMeasure): Promise<void> {
    const confirmed = await this.dialogService.confirmDialog(
      'XÁC NHẬN XÓA',
      `Bạn chắc chắn muốn xóa đơn vị "${unitOfMeasure.name}"?`
    )
    if (confirmed) {
      this.uomService.deleteUnitOfMeasure(unitOfMeasure.id).subscribe({
        next: () => {
          this.unitOfMeasuresList = this.unitOfMeasuresList.filter(uom => uom.id !== unitOfMeasure.id)
          this.toastr.success(`Xóa thể đơn vị "${unitOfMeasure.name}" thành công`)
        },
        error: error => {
          console.log("Có lỗi xảy ra: ", error)
        }
      })
    }
  }

  onSearch() {
    const params = this.uomService.getUnitOfMeasureParams()
    params.search = this.searchTerm
    params.pageIndex = 1
    this.uomService.setUnitOfMeasureParams(params)
    this.adminUnitOfMeasureParams = params
    this.getAllUnitOfMeasuresForAdmin()
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm = ''
    }
    this.adminUnitOfMeasureParams = new UnitOfMeasureParams()
    this.uomService.setUnitOfMeasureParams(this.adminUnitOfMeasureParams)
    this.getAllUnitOfMeasuresForAdmin()
  }

  onPageChange(event: PageEvent) {
    const params = this.uomService.getUnitOfMeasureParams()
    params.pageIndex = event.pageIndex + 1
    params.pageSize = event.pageSize
    this.uomService.setUnitOfMeasureParams(params)
    this.adminUnitOfMeasureParams = params
    this.getAllUnitOfMeasuresForAdmin()
  }
}
