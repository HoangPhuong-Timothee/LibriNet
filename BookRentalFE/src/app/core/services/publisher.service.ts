import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, tap } from "rxjs";
import { environment } from 'src/environments/environment';
import { Pagination } from '../models/pagination.model';
import { PublisherParams } from '../models/params.model';
import { AddPublisherRequest, Publisher, UpdatePublisherRequest } from '../models/publisher.model';

@Injectable({
  providedIn: 'root'
})
export class PublisherService {

  publishers: Publisher[] = []
  publisherList: Publisher[] = []
  publisherPagination?: Pagination<Publisher[]>
  publisherParams = new PublisherParams()
  publisherCache = new Map<string, Pagination<Publisher[]>>()

  constructor(private http: HttpClient) { }

  getAllPublishers() {
    if (this.publishers.length > 0) {
      return of(this.publishers)
    }
    return this.http.get<Publisher[]>(`${environment.baseAPIUrl}/api/Publishers`)
  }

  getPublishersForAdmin(useCache = true): Observable<Pagination<Publisher[]>> {
    if (!useCache) {
      this.publisherCache.clear()
    }
    if (this.publisherCache.size > 0 && useCache) {
      if (this.publisherCache.has(Object.values(this.publisherParams).join('-'))) {
        this.publisherPagination = this.publisherCache.get(Object.values(this.publisherParams).join('-'))
        if(this.publisherPagination) {
          return of(this.publisherPagination)
        }
      }
    }
    let params = new HttpParams()
    if (this.publisherParams.search) params = params.append('search', this.publisherParams.search)
    params = params.append('pageIndex', this.publisherParams.pageIndex)
    params = params.append('pageSize', this.publisherParams.pageSize)
    return this.http.get<Pagination<Publisher[]>>(`${environment.baseAPIUrl}/api/Publishers/admin/publishers-list`, { params }).pipe(
      map(response => {
        this.publisherList = [...this.publisherList, ...response.data]
        this.publisherPagination = response
        return response
      })
    )
  }

  setPublisherParams(params: PublisherParams) {
    this.publisherParams = params
  }

  getPublisherParams() {
    return this.publisherParams
  }

  addNewPublisher(request: AddPublisherRequest): Observable<any> {
    return this.http.post(`${environment.baseAPIUrl}/api/Publishers`, request).pipe(
      tap(() => {
        this.publisherCache = new Map()
      })
    )
  }

  importPublishersFromFile(file: File): Observable<any> {
    const formData = new FormData()
    formData.append('file', file, file.name)
    return this.http.post(`${environment.baseAPIUrl}/api/Publishers/import-from-file`, formData).pipe(
      tap(() => {
        this.publisherCache = new Map()
      })
    )
  }

  updatePublisher(request: UpdatePublisherRequest): Observable<any> {
    return this.http.put(`${environment.baseAPIUrl}/api/Publishers/${request.id}`, request).pipe(
      tap(() => {
        this.publisherCache = new Map()
      })
    )
  }

  deletePublisher(id: number) {
    return this.http.delete(`${environment.baseAPIUrl}/api/Publishers/${id}`)
  }

  checkPublisherExist(name: string) {
    return this.http.get<boolean>(`${environment.baseAPIUrl}/api/Publishers/publisher-exists?name=${name}`)
  }

}
