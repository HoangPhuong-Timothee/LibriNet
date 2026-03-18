import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, tap } from "rxjs";
import { environment } from 'src/environments/environment';
import { AddGenreRequest, Genre, UpdateGenreRequest } from '../models/genre.model';
import { Pagination } from '../models/pagination.model';
import { GenreParams } from '../models/params.model';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  genres: Genre[] = []
  genresList: Genre[] = []
  genrePagination?: Pagination<Genre[]>
  genreParams = new GenreParams()
  genreCache = new Map<string, Pagination<Genre[]>>()

  constructor(private http: HttpClient) { }
  getAllGenres () {
    if (this.genres.length > 0) {
      return of(this.genres)
    }
    return this.http.get<Genre[]>(`${environment.baseAPIUrl}/api/Genres`)
  }
  getGenresForAdmin(useCache = true): Observable<Pagination<Genre[]>> {
    if (!useCache) {
      this.genreCache.clear()
    }
    if (this.genreCache.size > 0 && useCache) {
      if (this.genreCache.has(Object.values(this.genreParams).join('-'))) {
        this.genrePagination = this.genreCache.get(Object.values(this.genreParams).join('-'))
        if(this.genrePagination) {
          return of(this.genrePagination)
        }
      }
    }
    let params = new HttpParams()
    if (this.genreParams.search) params = params.append('search', this.genreParams.search)
    params = params.append('pageIndex', this.genreParams.pageIndex)
    params = params.append('pageSize', this.genreParams.pageSize)
    return this.http.get<Pagination<Genre[]>>(`${environment.baseAPIUrl}/api/Genres/admin/genres-list`, { params }).pipe(
      map(response => {
        this.genresList = [...this.genresList, ...response.data]
        this.genrePagination = response
        return response
      })
    )
  }
  setGenreParams(params: GenreParams) {
    this.genreParams = params
  }
  getGenreParams() {
    return this.genreParams
  }
  addNewGenre(request: AddGenreRequest): Observable<any> {
    return this.http.post(`${environment.baseAPIUrl}/api/Genres`, request).pipe(
      tap(() => {
        this.genreCache = new Map()
      })
    )
  }
  importGenresFromFile(file: File): Observable<any> {
    const formData = new FormData()
    formData.append('file', file, file.name)
    return this.http.post(`${environment.baseAPIUrl}/api/Genres/import-from-file`, formData).pipe(
      tap(() => {
        this.genreCache = new Map()
      })
    )
  }
  updateGenre(request: UpdateGenreRequest) {
    return this.http.put(`${environment.baseAPIUrl}/api/Genres/${request.id}`, request).pipe(
      tap(() => {
        this.genreCache = new Map()
      })
    )
  }
  deleteGenre(id: number) {
    return this.http.delete(`${environment.baseAPIUrl}/api/Genres/soft-delete/${id}`)
  }
  checkGenreExist(name: string) {
    return this.http.get(`${environment.baseAPIUrl}/api/Genres/genre-exists?name=${name}`)
  }
}
