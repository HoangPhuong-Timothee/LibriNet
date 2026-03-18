import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { BookService } from 'src/app/core/services/book.service';

@Component({
  selector: 'app-add-books-form',
  templateUrl: './add-books-form.component.html',
  styleUrls: ['./add-books-form.component.css'],
})
export class AddBooksFormComponent {

  fileName: string = 'Chọn file tải lên.'
  errorsList: any[] = []
  selectedFile: File | null = null
  columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    private bookService: BookService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<AddBooksFormComponent>,
    private toastr: ToastrService
  ) { }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0]
    this.fileName = this.selectedFile ? this.selectedFile.name : 'Chọn file tải lên.'
  }

  onSubmit() {
    if (!this.selectedFile) {
      this.toastr.warning('Chưa có file nào được tải lên.')
      return
    }
    this.bookService.importBooksFromFile(this.selectedFile).subscribe({
      next: (response) => {
        this.toastr.success(response.message)
        this.dialogRef.close({ success: true })
      },
      error: (error) => {
        if (error.statusCode === 400) {
          this.toastr.error(error.message)
          this.errorsList = error.errors
        } else {
          this.toastr.error('Lỗi không xác định! Vui lòng thử lại.')
          this.dialogRef.close()
        }
      }
    })
  }

}
