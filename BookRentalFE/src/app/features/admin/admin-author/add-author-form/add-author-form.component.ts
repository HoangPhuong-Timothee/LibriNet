import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AddAuthorRequest } from 'src/app/core/models/author.model';
import { AuthorService } from 'src/app/core/services/author.service';
import { validateAuthorExist } from 'src/app/shared/helpers/validates/validate-exist';

@Component({
  selector: 'app-add-author-form',
  templateUrl: './add-author-form.component.html',
  styleUrls: ['./add-author-form.component.css']
})
export class AddAuthorFormComponent {

  constructor(
    private fb: FormBuilder,
    private authorService: AuthorService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<AddAuthorFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  addAuthorForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(50)], [validateAuthorExist(this.authorService)]]
  })

  addNewAuthor(): void {
    if(this.addAuthorForm.valid) {
      let addAuthorRequest = this.addAuthorForm.value as AddAuthorRequest
      this.authorService.addNewAuthor(addAuthorRequest).subscribe({
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
