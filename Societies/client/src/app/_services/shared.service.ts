import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor(public http: HttpClient, public toastr: ToastrService, public router: Router) { }

  public setUser(model: any){
    localStorage.setItem('user', model);
  }

  public getUser(){
    return localStorage.getItem('user');
  }

  public removeUser(){
    localStorage.removeItem('user');
  }

  public baseUrl = 'https://localhost:5001/api/';

  public hasApplication: boolean;
  public siteTelephone: string = "3214567";
}
