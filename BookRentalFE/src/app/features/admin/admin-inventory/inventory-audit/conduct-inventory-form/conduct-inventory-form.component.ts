import { Component, Inject, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { ConductInventoryRequest, InventoryAuditDetails } from 'src/app/core/models/inventory.model';
import { UnitOfMeasure } from 'src/app/core/models/unit-of-measure.model';
import { InventoryService } from 'src/app/core/services/inventory.service';
import { UnitOfMeasureService } from 'src/app/core/services/unit-of-measure.service';

@Component({
  selector: 'app-conduct-inventory-form',
  templateUrl: './conduct-inventory-form.component.html',
  styleUrls: ['./conduct-inventory-form.component.css']
})
export class ConductInventoryFormComponent implements OnInit {

  conductInventoryAuditForm!: FormGroup
  measureUnitList: UnitOfMeasure[] = []
  headerColumns: string[] = ['Sách', 'ISBN', 'Hiệu sách', 'Số lượng hệ thống', 'ĐVT', 'Số lượng thực tế', 'ĐVT']

  constructor(
    private inventoryService: InventoryService,
    private toastr: ToastrService,
    private measureUnitService: UnitOfMeasureService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<ConductInventoryFormComponent>
  ) { }

  ngOnInit(): void {
    this.loadAllMeasureUnits()
    this.initializeForm()
  }

  initializeForm() {
    this.conductInventoryAuditForm = this.fb.group({
      items: this.fb.array([], [Validators.required]),
    })
    this.addItem()
  }

  loadAllMeasureUnits() {
    this.measureUnitService.getAllUnitOfMeasures().subscribe({
      next: response => this.measureUnitList = response,
      error: error => console.log("Có lỗi xảy ra: ", error)
    })
  }

  createFormGroup(detail: InventoryAuditDetails): FormGroup {
    return this.fb.group({
      bookTitle: [{ value: detail.bookTitle, disabled: true }, [Validators.required]],
      isbn: [{ value: detail.isbn, disabled: true }, [Validators.required]],
      storeName: [{ value: detail.storeName, disabled: true }, [Validators.required]],
      inventoryQuantity: [{ value: detail.inventoryQuantity, disabled: true }, [Validators.required]],
      defaultSystemMeasureUnit: [{ value: detail.measureUnit, disabled: true }],
      actualQuantity: [0, [Validators.required, Validators.min(0)]],
      unitOfMeasureId: ['', [Validators.required]]
    })
  }

  addItem() {
    this.data.details.forEach((detail: InventoryAuditDetails) => {
      this.items.push(this.createFormGroup(detail))
    })
  }

  get items(): FormArray {
    return this.conductInventoryAuditForm.get('items') as FormArray
  }

  submitForm() {
    if (this.conductInventoryAuditForm.valid) {
      const requestData: ConductInventoryRequest[] = this.conductInventoryAuditForm.value.items
      console.log(requestData)
      // this.inventoryService.conductInventoryAudit(this.data.invAudit.id, conductInventoryRequest).subscribe({
      //   next: (response) => {
      //     this.toastr.success(response.message)
      //     this.dialogRef.close({ success: true })
      //   },
      //   error: (error) => {
      //     console.log("Có lỗi: ", error)
      //     this.toastr.error(error.message)
      //     this.dialogRef.close()
      //   }
      // })
    }
  }
}
