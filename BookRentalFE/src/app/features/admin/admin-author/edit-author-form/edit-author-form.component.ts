import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { UpdateAuthorRequest } from 'src/app/core/models/author.model';
import { AuthorService } from 'src/app/core/services/author.service';

@Component({
  selector: 'app-edit-author-form',
  templateUrl: './edit-author-form.component.html',
  styleUrls: ['./edit-author-form.component.css']
})
export class EditAuthorFormComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private authorService: AuthorService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<EditAuthorFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
    if (this.data.author) {
      this.updateAuthorForm.reset(this.data.author)
    }
  }

  updateAuthorForm = this.fb.group({
    id: [this.data.id],
    name: ['', [Validators.required, Validators.maxLength(50)]]
  })

  updateAuthor(): void {
    if(this.updateAuthorForm.valid) {
      let updateAuthorRequest = this.updateAuthorForm.value as UpdateAuthorRequest
      this.authorService.updateAuthor(updateAuthorRequest).subscribe({
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
