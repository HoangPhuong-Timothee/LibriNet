import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private authService: AuthService, private toastr: ToastrService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> {
    return this.authService.currentUser$.pipe(
      map((auth) => {
        if (auth) { 
          const roles = auth?.roles
          const isAdmin = Array.isArray(roles) ? roles.includes('Admin') : roles === 'Admin'
          if (isAdmin) {
            return true
          } else {
            this.router.navigate(['/'], { queryParams: { returnUrl: state.url } })
            this.toastr.error("Không có quyền truy cập và thực hiện chức năng!")
            return false
          }
        } else {
          this.toastr.info("Vui lòng đăng nhập để thực hiện chức năng!")
          this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } })
          return false
        }
      })
    )
  }
  
}
