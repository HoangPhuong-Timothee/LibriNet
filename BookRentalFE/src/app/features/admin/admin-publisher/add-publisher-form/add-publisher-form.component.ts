import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AddPublisherRequest } from 'src/app/core/models/publisher.model';
import { PublisherService } from 'src/app/core/services/publisher.service';
import { validatePublisherExist } from 'src/app/shared/helpers/validates/validate-exist';

@Component({
  selector: 'app-add-publisher-form',
  templateUrl: './add-publisher-form.component.html',
  styleUrls: ['./add-publisher-form.component.css']
})
export class AddPublisherFormComponent {

  constructor(
    private fb: FormBuilder,
    private publisherService: PublisherService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<AddPublisherFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  addPublisherForm = this.fb.group({
    name: ['', [Validators.required], [validatePublisherExist(this.publisherService)]],
    address: ['', [Validators.required, Validators.maxLength(255)]]
  })

  addNewPublisher(): void {
    if(this.addPublisherForm.valid) {
      let addPublisherRequest = this.addPublisherForm.value as AddPublisherRequest
      this.publisherService.addNewPublisher(addPublisherRequest).subscribe({
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
