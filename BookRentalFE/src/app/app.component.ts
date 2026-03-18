import { Component, OnInit } from '@angular/core';
import { AuthService } from './core/services/auth.service';
import { BasketService } from './core/services/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private basketService: BasketService
  ) { }

  ngOnInit(): void {
    this.loadCurrentUser()
    this.loadBasket()
  }

  loadCurrentUser(): void {
    const token = localStorage.getItem('access_token')
    this.authService.loadCurrentUser(token).subscribe()
  }

  loadBasket(): void {
    const basketKey = localStorage.getItem('basket_key')
    if (basketKey) {
      this.basketService.getBasket(basketKey)
    }
  }

}
