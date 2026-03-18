import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { ToastrService } from 'ngx-toastr';
import { map, Observable, of, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginRequest, RefreshTokenRequest, RegisterRequest } from '../models/auth.model';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private currentUserSource = new ReplaySubject<User | null>(1)
  currentUser$ = this.currentUserSource.asObservable()

  constructor(
    private http: HttpClient,
    private router: Router,
    private toastr: ToastrService
  ) {}

  login(request: LoginRequest) {
    return this.http.post<User>(`${environment.baseAPIUrl}/api/Auth/login`, request).pipe(
      map((user) => {
        if (user) {
          this.currentUserSource.next(user)
          localStorage.setItem('access_token', user.token)
        }
      })
    )
  }

  loadCurrentUser(token: string | null) {
    if (token === null) {
      this.currentUserSource.next(null)
      return of(null)
    }
    const decodedToken = jwtDecode(token)
    const expirationDate = decodedToken.exp ? decodedToken.exp * 1000 : null
    const currentDate = new Date().getTime()
    if (expirationDate && currentDate > expirationDate) {
      localStorage.removeItem('access_token')
      this.toastr.warning('Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại')
      this.currentUserSource.next(null)
      return of(null)
    } else if (expirationDate) {
      let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`)
      return this.http.get<User>(`${environment.baseAPIUrl}/api/Users/current-user`, { headers }).pipe(
      map((user) => {
        if (user) {
          this.currentUserSource.next(user)
          return user
        } else {
          return null
        }
      })
      )
    } else {
      this.toastr.warning('Có lỗi xác thực. Vui lòng đăng nhập lại.')
      localStorage.removeItem('access_token')
      this.currentUserSource.next(null)
      return of(null)
    }
  }

  get isAdmin$(): Observable<boolean> {
    return this.currentUser$.pipe(
      map((user) => {
        const roles = user?.roles
        return Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin'
      })
    )
  }

  register(request: RegisterRequest): Observable<any> {
    return this.http.post(`${environment.baseAPIUrl}/api/Auth/register`, request)
  }

  logout() {
    localStorage.removeItem('access_token');
    this.currentUserSource.next(null)
    setTimeout(() => {
      this.router.navigateByUrl('/')
      this.toastr.success('Đã đăng xuất tài khoản')
    }, 800)
  }

 checkEmailExists(email: string) {
  return this.http.get<boolean>(`${environment.baseAPIUrl}/api/Auth/email-exists?email=${email}`)
 }

 refreshToken(request: RefreshTokenRequest) {
  const accessToken = localStorage.getItem('access_token')
  return this.http.post(`${environment.baseAPIUrl}/api/Auth/token/refresh`, request)
 }

}
