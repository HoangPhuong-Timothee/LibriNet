import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Pagination } from '../models/pagination.model';
import { UnitOfMeasureParams } from '../models/params.model';
import { AddUnitOfMeasureRequest, UnitOfMeasure, UpdateUnitOfMeasureRequest } from '../models/unit-of-measure.model';

@Injectable({
  providedIn: 'root'
})
export class UnitOfMeasureService {

  unitOfMeasures: UnitOfMeasure[] = []
  unitOfMeasuresList: UnitOfMeasure[] = []
  unitOfMeasurePagination?: Pagination<UnitOfMeasure[]>
  unitOfMeasureParams = new UnitOfMeasureParams()
  unitOfMeasureCache = new Map<string, Pagination<UnitOfMeasure[]>>()

  constructor(private http: HttpClient) { }

  getUnitOfMeasuresForAdmin(useCache = true): Observable<Pagination<UnitOfMeasure[]>> {
    if (!useCache) {
      this.unitOfMeasureCache.clear()
    }
    if (this.unitOfMeasureCache.size > 0 && useCache) {
      if (this.unitOfMeasureCache.has(Object.values(this.unitOfMeasureParams).join('-'))) {
        this.unitOfMeasurePagination = this.unitOfMeasureCache.get(Object.values(this.unitOfMeasureParams).join('-'))

        if(this.unitOfMeasurePagination) {
          return of(this.unitOfMeasurePagination)
        }
      }
    }
    let params = new HttpParams()
    if (this.unitOfMeasureParams.search) params = params.append('search', this.unitOfMeasureParams.search)
    params = params.append('pageIndex', this.unitOfMeasureParams.pageIndex)
    params = params.append('pageSize', this.unitOfMeasureParams.pageSize)
    return this.http.get<Pagination<UnitOfMeasure[]>>(`${environment.baseAPIUrl}/api/UnitOfMeasures/admin/unit-of-measures-list`, { params }).pipe(
      map(response => {
        this.unitOfMeasuresList = [...this.unitOfMeasuresList, ...response.data]
        this.unitOfMeasurePagination = response
        return response
      })
    )
  }

  setUnitOfMeasureParams(params: UnitOfMeasureParams) {
    this.unitOfMeasureParams = params
  }

  getUnitOfMeasureParams() {
    return this.unitOfMeasureParams
  }

  getAllUnitOfMeasures() {
    if (this.unitOfMeasures.length > 0) {
      return of(this.unitOfMeasures)
    }
    return this.http.get<UnitOfMeasure[]>(`${environment.baseAPIUrl}/api/UnitOfMeasures`)
  }

  addNewUnitOfMeasure(request: AddUnitOfMeasureRequest): Observable<any> {
    return this.http.post(`${environment.baseAPIUrl}/api/UnitOfMeasures`, request).pipe(
      tap(() => {
        this.unitOfMeasureCache = new Map()
      })
    )
  }

  importUnitOfMeasuresFromFile(file: File): Observable<any> {
    const formData = new FormData()
    formData.append('file', file, file.name)
    return this.http.post(`${environment.baseAPIUrl}/api/UnitOfMeasures/import-from-file`, formData).pipe(
      tap(() => {
        this.unitOfMeasureCache = new Map()
      })
    )
  }

  updateUnitOfMeasure(request: UpdateUnitOfMeasureRequest): Observable<any> {
    return this.http.put(`${environment.baseAPIUrl}/api/UnitOfMeasures/${request.id}`, request).pipe(
      tap(() => {
        this.unitOfMeasureCache = new Map()
      })
    )
  }

  deleteUnitOfMeasure(id: number) {
    return this.http.delete(`${environment.baseAPIUrl}/api/UnitOfMeasures/soft-delete/${id}`)
  }

  checkUnitOfMeasureExist(name: string) {
    return this.http.get(`${environment.baseAPIUrl}/api/UnitOfMeasures/unit-of-measure-exists?name=${name}`)
  }

}
