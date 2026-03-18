import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { delay, finalize, identity, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { WaitingService } from '../services/waiting.service';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {

  constructor(private waitingService: WaitingService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    if (
        request.url.includes('email-exists') ||
        request.url.includes('book-exists') ||
        request.url.includes('exists-in-bookstore') ||
        request.url.includes('genre-exists') ||
        request.url.includes('author-exists') ||
        request.url.includes('publisher-exists') ||
        request.url.includes('bookstore-in-bookstore') ||
        request.url.includes('unit-of-measure-exist') ||
        request.url.includes('check-isbn') ||
        request.url.includes('quantity') ||
        request.method === 'DELETE' ||
        request.method === 'POST' && request.url.includes('Orders')
      )
      {
        return next.handle(request)
      }

      this.waitingService.waiting()
      return next.handle(request).pipe(
        (environment.production ? identity : delay(1000)),
        finalize(() => this.waitingService.idle())
      )
  }
}
