import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UnitOfMeasureService } from 'src/app/core/services/unit-of-measure.service';

@Component({
  selector: 'app-import-measure-unit-form',
  templateUrl: './import-measure-unit-form.component.html',
  styleUrls: ['./import-measure-unit-form.component.css']
})
export class ImportMeasureUnitFormComponent {

  columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    private uomService: UnitOfMeasureService,
    private dialogRef: MatDialogRef<ImportMeasureUnitFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  handleSubmitSuccess(success: boolean) {
    if (success) {
      this.dialogRef.close({ importSuccess: true })
    }
  }

  importMeasureUnitsFromFile(file: File) {
    return this.uomService.importUnitOfMeasuresFromFile(file)
  }
}
