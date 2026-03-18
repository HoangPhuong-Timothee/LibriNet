import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-file-input',
  templateUrl: './file-input.component.html',
  styleUrls: ['./file-input.component.css']
})
export class FileInputComponent {

  @Input() columns: any[] = []
  @Input() title: string = ''
  @Input() serviceMethod!: (file: File) => any
  @Output() importSuccess = new EventEmitter<boolean>()
  fileName: string = 'Chọn file tải lên.'
  errorsList: any[] = []
  selectedFile: File | null = null

  constructor(private toastr: ToastrService) { }

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0]
    this.fileName = this.selectedFile ? this.selectedFile.name : 'Chọn file tải lên.'
  }

  onSubmit() {
    if (!this.selectedFile) {
      this.toastr.warning('Chưa có file nào được tải lên')
      return
    }
    this.serviceMethod(this.selectedFile).subscribe({
      next: (response: any) => {
        this.toastr.success(response.message)
        this.importSuccess.emit(true)
        this.errorsList = []
      },
      error: (error: any) => {
        if (error.statusCode === 400) {
          this.toastr.error(error.message)
          this.errorsList = error.errors
        } else {
          this.toastr.error('Lỗi không xác định! Vui lòng thử lại.')
          this.importSuccess.emit(false)
        }
      }
    })
  }
}

