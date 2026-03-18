import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { map, Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
      return this.authService.currentUser$.pipe(
        map(auth => {
          if (auth) {
            return true
          } else {
            this.toastr.info("Vui lòng đăng nhập để thực hiện chức năng!")
            this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } })
            return false
          }
        })
      )
  }
  
}
