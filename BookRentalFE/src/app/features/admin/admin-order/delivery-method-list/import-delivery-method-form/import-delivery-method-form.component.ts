import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { OrderService } from 'src/app/core/services/order.service';

@Component({
  selector: 'app-import-delivery-method-form',
  templateUrl: './import-delivery-method-form.component.html',
  styleUrls: ['./import-delivery-method-form.component.css']
})
export class ImportDeliveryMethodFormComponent {

  columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    private orderService: OrderService,
    private dialogRef: MatDialogRef<ImportDeliveryMethodFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  handleSubmitSuccess(success: boolean) {
    if (success) {
      this.dialogRef.close({ importSuccess: true })
    }
  }

  importDmsFromFile(file: File) {
    return this.orderService.importDmsFromFile(file)
  }
}
