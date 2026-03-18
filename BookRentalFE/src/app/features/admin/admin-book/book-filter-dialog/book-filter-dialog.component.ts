import { Component, Inject, OnInit } from '@angular/core'
import { FormControl } from '@angular/forms'
import { MatOptionSelectionChange } from '@angular/material/core'
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog'
import { debounceTime, map, Observable, of, startWith, switchMap } from 'rxjs'
import { Author } from 'src/app/core/models/author.model'
import { Genre } from 'src/app/core/models/genre.model'
import { Publisher } from 'src/app/core/models/publisher.model'
import { AuthorService } from 'src/app/core/services/author.service'
import { GenreService } from 'src/app/core/services/genre.service'
import { PublisherService } from 'src/app/core/services/publisher.service'

@Component({
  selector: 'app-book-filter-dialog',
  templateUrl: './book-filter-dialog.component.html',
  styleUrls: ['./book-filter-dialog.component.css']
})
export class BookFilterDialogComponent implements OnInit {

  genresList: Genre[] = []
  publisherList: Publisher[] = []
  authorControl = new FormControl()
  filteredAuthors!: Observable<Author[]>
  selectedGenreId: number = this.data.selectedGenreId
  selectedPublisherId: number = this.data.selectedPublisherId
  selectedAuthorId: number = this.data.selectedAuthorId

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    public genreService: GenreService,
    public authorService: AuthorService,
    private publisherService: PublisherService,
    private filterDialogRef: MatDialogRef<BookFilterDialogComponent>
  ) { }

  ngOnInit(): void {
    this.loadAllPublishers()
    this.loadAllGenres()
    this.filteredAuthors = this.authorControl.valueChanges.pipe(
      startWith(''),
      debounceTime(500),
      switchMap(value => this.filterAuthors(value || ''))
    )
  }

  filterAuthors(value: string): Observable<Author[]> {
    if (value.length < 3) {
      return of([])
    }
    return this.authorService.getAllAuthors(value).pipe(
      map(response =>
        response.filter(author => author.name.toLowerCase().normalize('NFD').replace(/[\u0300-\u036f]/g, '').includes(value)
      ))
    )
  }

  loadAllGenres() {
    this.genreService.getAllGenres().subscribe({
      next: response => {
        this.genresList = response
      }
    })
  }

  loadAllPublishers() {
    this.publisherService.getAllPublishers().subscribe({
      next: response => {
        this.publisherList = response
      }
    })
  }

  applyFilters() {
    this.filterDialogRef.close({
      selectedGenreId: this.selectedGenreId,
      selectedPublisherId: this.selectedPublisherId,
      selectedAuthorId: this.selectedAuthorId
    })
  }

  onAuthorSelected(event: MatOptionSelectionChange, author: Author) {
    if (event.isUserInput) {
      this.selectedAuthorId = author.id
    }
  }

  displayAuthorName(author?: Author): string {
    return author ? author.name : ''
  }
}
