import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { AddGenreRequest } from 'src/app/core/models/genre.model';
import { GenreService } from 'src/app/core/services/genre.service';
import { validateGenreExist } from 'src/app/shared/helpers/validates/validate-exist';

@Component({
  selector: 'app-add-genre-form',
  templateUrl: './add-genre-form.component.html',
  styleUrls: ['./add-genre-form.component.css']
})
export class AddGenreFormComponent {

  constructor(
    private fb: FormBuilder,
    private genreService: GenreService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<AddGenreFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  addGenreForm = this.fb.group({
    name: ['', [Validators.required], [validateGenreExist(this.genreService)]]
  })

  addNewGenre(): void {
    if(this.addGenreForm.valid) {
      let addGenreRequest = this.addGenreForm.value as AddGenreRequest
      this.genreService.addNewGenre(addGenreRequest).subscribe({
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
