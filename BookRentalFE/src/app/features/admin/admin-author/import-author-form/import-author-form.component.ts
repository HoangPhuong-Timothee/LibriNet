import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AuthorService } from 'src/app/core/services/author.service';

@Component({
  selector: 'app-import-author-form',
  templateUrl: './import-author-form.component.html',
  styleUrls: ['./import-author-form.component.css']
})
export class ImportAuthorFormComponent {

  columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    private authorService: AuthorService,
    private dialogRef: MatDialogRef<ImportAuthorFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  handleSubmitSuccess(success: boolean) {
    if (success) {
      this.dialogRef.close({ importSuccess: true })
    }
  }

  importAuthorsFromFile(file: File) {
    return this.authorService.importAuthorsFromFile(file)
  }
}
