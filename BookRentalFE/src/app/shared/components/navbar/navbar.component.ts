import { Component } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { BasketService } from 'src/app/core/services/basket.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {

  basketItemCount$? = this.basketService.basketItemCount$

  constructor(
    public authService: AuthService,
    public basketService: BasketService
  ) { }

}
