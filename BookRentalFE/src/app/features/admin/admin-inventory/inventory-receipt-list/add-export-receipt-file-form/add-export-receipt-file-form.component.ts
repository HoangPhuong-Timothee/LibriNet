import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { InventoryService } from 'src/app/core/services/inventory.service';

@Component({
  selector: 'app-add-export-receipt-file-form',
  templateUrl: './add-export-receipt-file-form.component.html',
  styleUrls: ['./add-export-receipt-file-form.component.css']
})
export class AddExportReceiptFileFormComponent {

  columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private inventoryService: InventoryService,
    public dialogRef: MatDialogRef<AddExportReceiptFileFormComponent>
  ) { }

  handleSubmitSuccess(success: boolean) {
    if (success) {
      this.dialogRef.close({ exportSuccess: true })
    }
  }

  exportInventoriesFromFile(file: File) {
    return this.inventoryService.exportInventoriesFromFile(file)
  }
}
