import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, tap } from "rxjs";
import { environment } from 'src/environments/environment';
import { AddAuthorRequest, Author, UpdateAuthorRequest } from '../models/author.model';
import { Pagination } from '../models/pagination.model';
import { AuthorParams } from '../models/params.model';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  authors: Author[] = []
  authorsList: Author[] = []
  authorPagination?: Pagination<Author[]>
  authorParams = new AuthorParams()
  authorCache = new Map<string, Pagination<Author[]>>()

  constructor(private http: HttpClient) { }

  getAllAuthors(searchTerm: string): Observable<Author[]> {
    if (this.authors.length > 0) {
      return of(this.authors);
    }
    let params = new HttpParams()
    if (searchTerm) params = params.append('searchTerm', searchTerm)
    return this.http.get<Author[]>(`${environment.baseAPIUrl}/api/Authors`, { params })
  }

  getAuthorsForAdmin(useCache = true) {
    if (!useCache) {
      this.authorCache = new Map()
    }
    if (this.authorCache.size > 0 && useCache) {
      if (this.authorCache.has(Object.values(this.authorParams).join('-'))) {
        this.authorPagination = this.authorCache.get(Object.values(this.authorParams).join('-'))
        if(this.authorPagination) {
          return of(this.authorPagination)
        }
      }
    }
    let params = new HttpParams()
    if (this.authorParams.search) params = params.append('search', this.authorParams.search)
    params = params.append('pageIndex', this.authorParams.pageIndex)
    params = params.append('pageSize', this.authorParams.pageSize)
    return this.http.get<Pagination<Author[]>>(`${environment.baseAPIUrl}/api/Authors/admin/authors-list`, { params }).pipe(
      map(response => {
        this.authorsList = [...this.authorsList, ...response.data]
        this.authorPagination = response
        return response
      })
    )
  }

  setAuthorParams(params: AuthorParams) {
    this.authorParams = params
  }

  getAuthorParams() {
    return this.authorParams
  }

  addNewAuthor(request: AddAuthorRequest): Observable<any> {
    return this.http.post(`${environment.baseAPIUrl}/api/Authors`, request).pipe(
      tap(() => {
        this.authorCache = new Map()
      })
    )
  }

  importAuthorsFromFile(file: File): Observable<any> {
    const formData = new FormData()
    formData.append('file', file, file.name)
    return this.http.post(`${environment.baseAPIUrl}/api/Authors/import-from-file`, formData).pipe(
      tap(() => {
        this.authorCache = new Map()
      })
    )
  }

  updateAuthor(request: UpdateAuthorRequest): Observable<any> {
    return this.http.put(`${environment.baseAPIUrl}/api/Authors/${request.id}`, request).pipe(
      tap(() => {
        this.authorCache = new Map()
      })
    )
  }

  deleteAuthor(id: number) {
    return this.http.delete(`${environment.baseAPIUrl}/api/Authors/${id}`)
  }

  checkAuthorExist(name: string) {
    return this.http.get(`${environment.baseAPIUrl}/api/Authors/author-exists?name=${name}`)
  }

}
