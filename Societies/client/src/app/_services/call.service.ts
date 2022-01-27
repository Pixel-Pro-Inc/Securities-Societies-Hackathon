import { Injectable } from '@angular/core';
import { MasterService } from './master.service';

@Injectable({
  providedIn: 'root'
})
export class CallService extends MasterService{

  constructor()
  {
    super();
  }

  call(num: string){
    window.open('tel:' + num);
  }
}
