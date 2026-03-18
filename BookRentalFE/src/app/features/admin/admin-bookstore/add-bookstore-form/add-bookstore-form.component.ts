import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AddBookStoreRequest } from 'src/app/core/models/book-store.model';
import { BookstoreService } from 'src/app/core/services/bookstore.service';
import { validateBookStoreExist } from 'src/app/shared/helpers/validates/validate-exist';

@Component({
  selector: 'app-add-bookstore-form',
  templateUrl: './add-bookstore-form.component.html',
  styleUrls: ['./add-bookstore-form.component.css']
})
export class AddBookstoreFormComponent {

  constructor(
    private fb: FormBuilder,
    private toastr: ToastrService,
    private bookStoreService: BookstoreService,
    private dialogRef: MatDialogRef<AddBookstoreFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  addBookStoreForm = this.fb.group({
    storeName: ['', [Validators.required], [validateBookStoreExist(this.bookStoreService)]],
    storeAddress: ['', [Validators.required]]
  })

  addNewBookStore() {
    if(this.addBookStoreForm.valid) {
      let addBookStoreRequest = this.addBookStoreForm.value as AddBookStoreRequest
      this.bookStoreService.addNewBookStore(addBookStoreRequest).subscribe({
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
