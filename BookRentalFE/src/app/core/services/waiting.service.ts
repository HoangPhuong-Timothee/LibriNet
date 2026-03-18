import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class WaitingService {

  waitingRequestCount = 0

  constructor(private spinnerService: NgxSpinnerService) { }

  waiting() {
    this.waitingRequestCount++
    this.spinnerService.show(undefined, {
      type: 'line-scale-pulse-out',
      bdColor: 'rgba(255,255,255,0.7)',
      color: '#333333'
    })
  }

  idle() {
    this.waitingRequestCount--
    if (this.waitingRequestCount <= 0) {
      this.waitingRequestCount = 0
      this.spinnerService.hide()
    }
  }
}
