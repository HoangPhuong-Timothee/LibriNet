import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BookstoreService } from 'src/app/core/services/bookstore.service';

@Component({
  selector: 'app-import-bookstore-form',
  templateUrl: './import-bookstore-form.component.html',
  styleUrls: ['./import-bookstore-form.component.css']
})
export class ImportBookstoreFormComponent {

  columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    private bookStoreService: BookstoreService,
    private dialogRef: MatDialogRef<ImportBookstoreFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  handleSubmitSuccess(success: boolean) {
    if (success) {
      this.dialogRef.close({ importSuccess: true })
    }
  }

  importBookStoresFromFile(file: File) {
    return this.bookStoreService.importBookStoresFromFile(file)
  }
}
