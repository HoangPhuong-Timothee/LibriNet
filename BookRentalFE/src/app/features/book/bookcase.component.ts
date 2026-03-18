import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/core/models/book.model';
import { Genre } from 'src/app/core/models/genre.model';
import { BookParams } from 'src/app/core/models/params.model';
import { Publisher } from 'src/app/core/models/publisher.model';
import { BookService } from 'src/app/core/services/book.service';
import { GenreService } from 'src/app/core/services/genre.service';
import { PublisherService } from 'src/app/core/services/publisher.service';

@Component({
  selector: 'app-bookcase',
  templateUrl: './bookcase.component.html',
  styleUrls: ['./bookcase.component.css']
})
export class BookcaseComponent implements OnInit {

  searchTerm: string = ''
  books?: Book[]
  genres: Genre[] = []
  publishers: Publisher[] = []
  bookParams: BookParams
  totalBooks = 0
  sortOptions = [
    { name: 'Theo bảng chữ cái', value: 'title' },
    { name: 'Giá: Tăng dần', value: 'priceAsc' },
    { name: 'Giá: Giảm dần', value: 'priceDesc' },
    { name: 'Mới nhất', value: 'latest' },
    { name: 'Cũ nhất', value: 'oldest' }
  ]

  constructor(
    private bookService: BookService,
    private genreService: GenreService,
    private publisherService: PublisherService
  )
  {
    this.bookParams = bookService.getBookParams()
  }

  ngOnInit(): void {
    this.getAllBooks()
    this.getAllGenres()
    this.getAllPublishers()
  }

  getAllBooks() {
    this.bookService.getAllBooks().subscribe({
      next: (response) => {
        this.books = response.data
        this.totalBooks = response.count
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

  getAllGenres(){
    this.genreService.getAllGenres().subscribe({
      next: (response) => {
        this.genres = [{id: 0, name: 'Tất cả'}, ...response];
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

  getAllPublishers(){
    this.publisherService.getAllPublishers().subscribe({
      next: (response) => {
        this.publishers = [{id: 0, name: 'Tất cả'}, ...response];
      },
      error: (error) => {
        console.log(error);
      }
    })
  }

  onSortSelected(event: any) {
    const params = this.bookService.getBookParams()
    params.sort = event.target.value
    this.bookService.setBookParams(params)
    this.bookParams = params
    this.getAllBooks()
  }

  onGenreSelected(event: any) {
    const params = this.bookService.getBookParams()
    const selectedGenreId = event.target.value
    params.genreId = selectedGenreId === '0' ? null : selectedGenreId
    params.pageIndex = 1
    this.bookService.setBookParams(params)
    this.bookParams = params
    this.getAllBooks()
  }

  onPublisherSelected(event: any) {
    const params = this.bookService.getBookParams()
    const selectedPublisherId = event.target.value
    params.publisherId = selectedPublisherId === '0' ? null : selectedPublisherId
    params.pageIndex = 1
    this.bookService.setBookParams(params)
    this.bookParams = params
    this.getAllBooks()
  }

  onPageChanged(event: any) {
    const params = this.bookService.getBookParams()
    if (params.pageIndex !== event) {
      params.pageIndex = event
      this.bookService.setBookParams(params)
      this.bookParams = params
      this.getAllBooks()
    }
  }

  onSearch() {
    const params = this.bookService.getBookParams()
    params.search = this.searchTerm
    params.pageIndex = 1
    this.bookService.setBookParams(params)
    this.bookParams = params
    this.getAllBooks()
  }

  onReset() {
    if (this.searchTerm) {
      this.searchTerm = ''
    }
    this.bookParams = new BookParams()
    this.bookService.setBookParams(this.bookParams)
    this.getAllBooks()
  }

}
