import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { UpdateBookStoreRequest } from 'src/app/core/models/book-store.model';
import { BookstoreService } from 'src/app/core/services/bookstore.service';

@Component({
  selector: 'app-edit-bookstore-form',
  templateUrl: './edit-bookstore-form.component.html',
  styleUrls: ['./edit-bookstore-form.component.css']
})
export class EditBookstoreFormComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private bookStoreService: BookstoreService,
    private toastr: ToastrService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<EditBookstoreFormComponent>
  ) { }

  ngOnInit(): void {
    if (this.data.bookStore) {
      this.updateBookStoreForm.reset(this.data.bookStore)
    }
  }

  updateBookStoreForm = this.fb.group({
    id: [this.data.bookStore.id],
    storeName: ['', [Validators.required]],
    storeAddress: ['', [Validators.required]]
  })

  updateBookStore() {
    if(this.updateBookStoreForm.valid) {
      let updateBookStoreRequest = this.updateBookStoreForm.value as UpdateBookStoreRequest
      this.bookStoreService.updateBookStore(updateBookStoreRequest).subscribe({
        next: (response) => {
          if (response) {
            this.toastr.success("Cập nhật hiệu sách thành công")
            this.dialogRef.close({ success: true })
          }
        },
        error: (error) => {
            console.log("Có lỗi xảy ra: ", error)
            this.toastr.error('Cập nhật hiệu sách thất bại!')
          }
      })
    }
  }

}
