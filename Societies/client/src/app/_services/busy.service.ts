import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
@Injectable({
  providedIn: 'root'
})
export class BusyService {

  busyRequestCount = 0;
  isLoading: Boolean;
  constructor(private spinnerService: NgxSpinnerService) {}

  busy(msg: string){
    localStorage.setItem('loadingMessage', msg);
    
    this.busyRequestCount++;
    this.spinnerService.show(undefined, {
      type: 'timer',
      bdColor: 'rgba(0, 0, 0, 0.8)',
      color: '#fff'
    });
  }

  idle(){
    this.busyRequestCount--;
    if(this.busyRequestCount <= 0){
      this.busyRequestCount = 0;
      this.spinnerService.hide();
    }
  }

}
