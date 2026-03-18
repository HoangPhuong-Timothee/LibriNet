import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, tap } from "rxjs";
import { environment } from 'src/environments/environment';
import { Book } from '../models/book.model';
import { Pagination } from '../models/pagination.model';
import { BookParams } from '../models/params.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {

  books: Book[] = []
  bookList: Book[] = []
  latestBooks: Book[] = []
  similarBooks: Book[] = []
  bookPagination?: Pagination<Book[]>
  adminBookPagination?: Pagination<Book[]>
  bookParams = new BookParams()
  adminBookParams = new BookParams()
  bookCache = new Map<string, Pagination<Book[]>>()
  adminBookCache = new Map<string, Pagination<Book[]>>()

  constructor(private http: HttpClient) { }

  getAllBooksForAdmin(useCache = true): Observable<Pagination<Book[]>> {
    if (!useCache) {
      this.adminBookCache = new Map()
    }
    if (this.adminBookCache.size > 0 && useCache) {
      if (this.adminBookCache.has(Object.values(this.adminBookParams).join('-'))) {
        this.adminBookPagination = this.adminBookCache.get(Object.values(this.adminBookParams).join('-'))

        if(this.adminBookPagination) {
          return of(this.adminBookPagination)
        }
      }
    }
    let params = new HttpParams()
    if (this.adminBookParams.genreId) params = params.append('genreId', this.adminBookParams.genreId)
    if (this.adminBookParams.authorId) params = params.append('authorId', this.adminBookParams.authorId)
    if (this.adminBookParams.publisherId) params = params.append('publisherId', this.adminBookParams.publisherId)
    if (this.adminBookParams.search) params = params.append('search', this.adminBookParams.search)
    params = params.append('pageIndex', this.adminBookParams.pageIndex)
    params = params.append('pageSize', this.adminBookParams.pageSize)
    return this.http.get<Pagination<Book[]>>(`${environment.baseAPIUrl}/api/Books`, { params }).pipe(
      map(response => {
        this.bookList = [...this.bookList, ...response.data]
        this.bookPagination = response
        return response
      })
    )
  }

  getAllBooks(useCache = true): Observable<Pagination<Book[]>> {
    if (!useCache) {
      this.bookCache = new Map()
    }
    if (this.bookCache.size > 0 && useCache) {
      if (this.bookCache.has(Object.values(this.bookParams).join('-'))) {
        this.bookPagination = this.bookCache.get(Object.values(this.bookParams).join('-'))

        if(this.bookPagination) {
          return of(this.bookPagination)
        }
      }
    }
    let params = new HttpParams()
    if (this.bookParams.genreId) params = params.append('genreId', this.bookParams.genreId)
    if (this.bookParams.publisherId) params = params.append('publisherId', this.bookParams.publisherId)
    if (this.bookParams.search) params = params.append('search', this.bookParams.search)
    params = params.append('sort', this.bookParams.sort)
    params = params.append('pageIndex', this.bookParams.pageIndex)
    params = params.append('pageSize', this.bookParams.pageSize)
    return this.http.get<Pagination<Book[]>>(`${environment.baseAPIUrl}/api/Books`, { params }).pipe(
      map(response => {
        this.books = [...this.books, ...response.data]
        this.bookPagination = response
        return response
      })
    )
  }

  setBookParams(params: BookParams) {
    this.bookParams = params
  }

  getBookParams() {
    return this.bookParams
  }

  setAdminBookParams(params: BookParams) {
    this.adminBookParams = params
  }

  getAdminBookParams() {
    return this.adminBookParams
  }

  getSingleBook(id: number): Observable<Book> {
    const book = [...this.bookCache.values()].reduce((acc, paginationResult) => {
      return {
        ...acc,
        ...paginationResult.data.find(x => x.id === id)
      }
    }, {} as Book)
    if (Object.keys(book).length !== 0) {
      return of(book)
    }
    return this.http.get<Book>(`${environment.baseAPIUrl}/api/Books/${id}`)
  }

  getSimilarBook(id: number): Observable<Book[]> {
    if (this.similarBooks.length > 0) {
      return of(this.similarBooks)
    }
    return this.http.get<Book[]>(`${environment.baseAPIUrl}/api/Books/${id}/similar`).pipe(
      map(response => this.similarBooks = response)
    )
  }

  getLatestBook(): Observable<Book[]> {
    if (this.latestBooks.length > 0) {
      return of(this.latestBooks)
    }
    return this.http.get<Book[]>(`${environment.baseAPIUrl}/api/Books/latest`).pipe(
      map(response => this.latestBooks = response)
    )
  }

  importBooksFromFile(file: File): Observable<any> {
    const formData = new FormData()
    formData.append('file', file, file.name)
    return this.http.post(`${environment.baseAPIUrl}/api/Books/import-from-file`, formData).pipe(
      tap(() => {
        this.adminBookCache = new Map()
      })
    )
  }

  deleteBook(id: number) {
    return this.http.delete(`${environment.baseAPIUrl}/api/Books/${id}`)
  }

  checkBookExistByTitle(title: string): Observable<boolean> {
    return this.http.get<boolean>(`${environment.baseAPIUrl}/api/Books/book-exists?bookTitle=${title}`)
  }

  checkBookExistInBookStore(title: string, bookStoreId: number): Observable<boolean> {
    return this.http.get<boolean>(`${environment.baseAPIUrl}/api/Books/exists-in-bookstore?bookTitle=${title}&bookStoreId=${bookStoreId}`)
  }

  checkBookISBN(isbn: string, title: string): Observable<boolean> {
    return this.http.get<boolean>(`${environment.baseAPIUrl}/api/Books/check-isbn?isbn=${isbn}&bookTitle=${title}`)
  }

  uploadBookImages(bookId: number, files: File[]): Observable<any> {
    const formData = new FormData()
    for (let file of files) {
      formData.append('files', file, file.name)
    }
    return this.http.post(`${environment.baseAPIUrl}/api/Books/${bookId}/upload-images`, formData)
  }

}
