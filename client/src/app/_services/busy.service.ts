import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root',
})
export class BusyService {
  busyRequestCount = 0;
  constructor(private spinnerServices: NgxSpinnerService) {}
  busy() {
    this.busyRequestCount++;
    this.spinnerServices.show(undefined, {
      type: 'ball-scale-multiple',
      bdColor: 'rgba(255,255,255,0)',
      color: '#333333',
    });
  }
  idly() {
    this.busyRequestCount--;
    if (this.busyRequestCount <= 0) {
      this.busyRequestCount = 0;
      this.spinnerServices.hide();
    }
  }
}
