import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { UpdateGenreRequest } from 'src/app/core/models/genre.model';
import { GenreService } from 'src/app/core/services/genre.service';

@Component({
  selector: 'app-edit-genre-form',
  templateUrl: './edit-genre-form.component.html',
  styleUrls: ['./edit-genre-form.component.css']
})
export class EditGenreFormComponent implements OnInit {

  constructor(
    private fb: FormBuilder,
    private genreService: GenreService,
    private toastr: ToastrService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<EditGenreFormComponent>
  ) { }

  ngOnInit(): void {
    if (this.data.genre) {
      this.updateGenreForm.reset(this.data.genre)
    }
  }

  updateGenreForm = this.fb.group({
    id: [this.data.genre.id],
    name: ['', [Validators.required, Validators.maxLength(20)]]
  })

  updateGenre() {
    if(this.updateGenreForm.valid) {
      let updateGenreRequest = this.updateGenreForm.value as UpdateGenreRequest
      this.genreService.updateGenre(updateGenreRequest).subscribe({
        next: (response) => {
          if (response) {
            this.toastr.success("Cập nhật thể loại thành công")
            this.dialogRef.close({ success: true })
          }
        },
        error: (error) => {
            console.log("Có lỗi xảy ra: ", error)
            this.toastr.error('Cập nhật thể loại thất bại!')
          }
      })
    }
  }

}
