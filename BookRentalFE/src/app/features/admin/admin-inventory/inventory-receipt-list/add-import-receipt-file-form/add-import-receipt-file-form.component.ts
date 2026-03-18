import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { InventoryService } from 'src/app/core/services/inventory.service';

@Component({
  selector: 'app-add-import-receipt-file-form',
  templateUrl: './add-import-receipt-file-form.component.html',
  styleUrls: ['./add-import-receipt-file-form.component.css']
})
export class AddImportReceiptFileFormComponent {

  columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private inventoryService: InventoryService,
    public dialogRef: MatDialogRef<AddImportReceiptFileFormComponent>
  ) { }

  handleSubmitSuccess(success: boolean) {
    if (success) {
      this.dialogRef.close({ importSuccess: true })
    }
  }

  importInventoriesFromFile(file: File) {
    return this.inventoryService.importInventoriesFromFile(file)
  }
}
