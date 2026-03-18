import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from './core/guards/admin.guard';
import { AuthGuard } from './core/guards/auth.guard';
import { HomeComponent } from './features/home/home.component';
import { IntroductionComponent } from './features/introduction/introduction.component';
import { TestErrorComponent } from './features/test-error/test-error.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { ServerErrorComponent } from './shared/components/server-error/server-error.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    data: { breadcrumb: 'Trang chủ' }
  },
  {
    path: 'admin',
    loadChildren: () => import('./features/admin/admin.module').then(m => m.AdminModule),
    canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: 'bookcase',
    loadChildren: () => import('./features/book/book.module').then(m => m.BookModule),
    data: { breadcrumb: 'Tủ sách' }
  },
  {
    path: '',
    loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'basket',
    loadChildren: () => import('./features/basket/basket.module').then(m => m.BasketModule),
    data: { breadcrumb: 'Giỏ sách' }
  },
  {
    path: 'profile',
    canActivate: [AuthGuard],
    loadChildren: () => import('./features/profile/profile.module').then(m => m.ProfileModule),
    data: { breadcrumb: 'Thông tin cá nhân' }
  },
  {
    path: 'checkout',
    canActivate: [AuthGuard],
    loadChildren: () => import('./features/checkout/checkout.module').then(m => m.CheckoutModule),
    data: { breadcrumb: 'Thanh toán' }
  },
  {
    path: 'order',
    canActivate: [AuthGuard],
    loadChildren: () => import('./features/order/order.module').then(m => m.OrderModule),
    data: { breadcrumb: 'Đơn hàng' }
  },
  {
    path: 'introduction',
    component: IntroductionComponent,
    data: { breadcrumb: 'Giới thiệu' }
  },
  {
    path: 'test-error',
    component: TestErrorComponent
  },
  {
    path: 'server-error',
    component: ServerErrorComponent
  },
  {
    path: 'not-found',
    component: NotFoundComponent
  },
  {
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
