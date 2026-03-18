import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BookService } from 'src/app/core/services/book.service';

@Component({
  selector: 'app-import-books-form',
  templateUrl: './import-books-form.component.html',
  styleUrls: ['./import-books-form.component.css']
})
export class ImportBooksFormComponent {
 columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    private bookService: BookService,
    private dialogRef: MatDialogRef<ImportBooksFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  handleSubmitSuccess(success: boolean) {
    if (success) {
      this.dialogRef.close({ importSuccess: true })
    }
  }

  importBooksFromFile(file: File) {
    return this.bookService.importBooksFromFile(file)
  }
}
