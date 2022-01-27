import { Injectable } from '@angular/core';
import { MasterService } from './master.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService extends MasterService {

  constructor() {
    super();
  }

  logout = () => localStorage.removeItem('user');
}
