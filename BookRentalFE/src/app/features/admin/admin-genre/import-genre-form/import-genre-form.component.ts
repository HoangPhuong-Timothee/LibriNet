import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { GenreService } from 'src/app/core/services/genre.service';

@Component({
  selector: 'app-import-genre-form',
  templateUrl: './import-genre-form.component.html',
  styleUrls: ['./import-genre-form.component.css']
})
export class ImportGenreFormComponent {

  columns = [
    { field: 'location', header: 'Vị trí' },
    { field: 'details', header: 'Nội dung' }
  ]

  constructor(
    private genreService: GenreService,
    private dialogRef: MatDialogRef<ImportGenreFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  handleSubmitSuccess(success: boolean) {
    if (success) {
      this.dialogRef.close({ importSuccess: true })
    }
  }

  importGenresFromFile(file: File) {
    return this.genreService.importGenresFromFile(file)
  }
}
