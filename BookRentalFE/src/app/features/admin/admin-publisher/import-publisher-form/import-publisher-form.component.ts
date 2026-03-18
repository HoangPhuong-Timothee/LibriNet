import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { PublisherService } from 'src/app/core/services/publisher.service';

@Component({
  selector: 'app-import-publisher-form',
  templateUrl: './import-publisher-form.component.html',
  styleUrls: ['./import-publisher-form.component.css']
})
export class ImportPublisherFormComponent {

  columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    private publisherService: PublisherService,
    private dialogRef: MatDialogRef<ImportPublisherFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  handleSubmitSuccess(success: boolean) {
    if (success) {
      this.dialogRef.close({ importSuccess: true })
    }
  }

  importPublishersFromFile(file: File) {
    return this.publisherService.importPublishersFromFile(file)
  }
}
