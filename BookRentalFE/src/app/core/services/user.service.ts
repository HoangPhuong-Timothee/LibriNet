import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Address, ModifyAddressRequest } from '../models/address.model';
import { Pagination } from '../models/pagination.model';
import { MemberParams } from '../models/params.model';
import { Member, ModifyProfileRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  membersList: Member[] = []
  memberPagination?: Pagination<Member[]>
  memberParams = new MemberParams()
  memberCache = new Map<string, Pagination<Member[]>>()

  constructor(private http: HttpClient) { }

  getMemberParams() {
    return this.memberParams
  }

  setMemberParams(params: MemberParams) {
    this.memberParams = params
  }

  getUsersList(useCache = true): Observable<Pagination<Member[]>> {
    if (!useCache) {
      this.memberCache = new Map()
    }
    if (this.memberCache.size > 0 && useCache) {
      if (this.memberCache.has(Object.values(this.memberParams).join('-'))) {
        this.memberPagination = this.memberCache.get(Object.values(this.memberParams).join('-'))
        if(this.memberPagination) {
          return of(this.memberPagination)
        }
      }
    }
    let params = new HttpParams()
    params = params.append('pageIndex', this.memberParams.pageIndex)
    params = params.append('pageSize', this.memberParams.pageSize)
    return this.http.get<Pagination<Member[]>>(`${environment.baseAPIUrl}/api/Users/admin/users-list`, { params }).pipe(
      map(response => {
        this.membersList = [...this.membersList,...response.data]
        this.memberPagination = response
        return response
      })
    )
  }

  modifyUserInfo(request: ModifyProfileRequest): Observable<any> {
    return this.http.put(`${environment.baseAPIUrl}/api/Users/profile`, request)
  }

  getUserAddress() {
    return this.http.get<Address>(`${environment.baseAPIUrl}/api/Users/address`)
  }

  modifyUserAddress(request: ModifyAddressRequest) {
    return this.http.put<Address>(`${environment.baseAPIUrl}/api/Users/address`, request)
  }

  uploadUserImage(file: File): Observable<any> {
    const formData = new FormData()
    formData.append('file', file, file.name)
    return this.http.post(`${environment.baseAPIUrl}/api/Users/upload-avatar`, formData)
  }
}
