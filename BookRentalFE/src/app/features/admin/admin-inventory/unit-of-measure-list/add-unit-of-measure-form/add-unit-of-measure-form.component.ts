import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AddUnitOfMeasureRequest, UnitOfMeasure } from 'src/app/core/models/unit-of-measure.model';
import { UnitOfMeasureService } from 'src/app/core/services/unit-of-measure.service';
import { validateUnitOfMeasureExist } from 'src/app/shared/helpers/validates/validate-exist';

@Component({
  selector: 'app-add-unit-of-measure-form',
  templateUrl: './add-unit-of-measure-form.component.html',
  styleUrls: ['./add-unit-of-measure-form.component.css']
})
export class AddUnitOfMeasureFormComponent implements OnInit {

    measureUnitsList: UnitOfMeasure[] = []

    constructor(
      private fb: FormBuilder,
      private toastr: ToastrService,
      private uomService: UnitOfMeasureService,
      private dialogRef: MatDialogRef<AddUnitOfMeasureFormComponent>,
      @Inject(MAT_DIALOG_DATA) public data: any
    ) { }

    addUnitOfMeasureForm = this.fb.group({
      name: ['', [Validators.required], [validateUnitOfMeasureExist(this.uomService)]],
      description: ['', [Validators.maxLength(255)]],
      conversionRate: [0, Validators.required],
      mappingUnitId: [0, [Validators.min(1)]]
    })

    ngOnInit(): void {
      this.loadAllMeasureUnits()
    }

    loadAllMeasureUnits() {
      this.uomService.getAllUnitOfMeasures().subscribe({
        next: response => this.measureUnitsList = response,
        error: error => console.log("Có lỗi xảy ra: ", error)
      })
    }

    addNewUnitOfMeasure(): void {
      if(this.addUnitOfMeasureForm.valid) {
        let addUnitOfMeasureRequest = this.addUnitOfMeasureForm.value as AddUnitOfMeasureRequest
        this.uomService.addNewUnitOfMeasure(addUnitOfMeasureRequest).subscribe({
          next: (response) => {
            this.toastr.success(response.message)
            this.dialogRef.close({ success: true })
          },
          error: (error) => {
            console.log("Có lỗi xảy ra: ", error)
            this.toastr.error(error.message)
          }
        })
      }
    }
}
