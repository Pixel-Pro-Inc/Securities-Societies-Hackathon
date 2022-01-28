import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { of } from 'rxjs';
import { SharedService } from './shared.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService{

  constructor(private shared: SharedService) {
  }

  omangFill(img: any){
    var post = this.shared.http.post(this.shared.baseUrl + 'account/omangfill', img);

    post.subscribe( response => {
      console.log(response);
    },
    error => {
      this.shared.toastr.error(error.error);
      return;
    });
    
    return post;
  }

  signup(model: any){
    this.shared.http.post(this.shared.baseUrl + 'account/signup', model).subscribe(
      response =>{
        this.shared.setUser(model);//Logs a user in
        this.shared.router.navigateByUrl('/');
      },
      error => {
        this.shared.toastr.error(error.error);
      }
    );
  }

  login(model: any){
    this.shared.http.post(this.shared.baseUrl + 'account/login', model).subscribe(
      response =>{
        this.shared.setUser(model);//Logs a user in
        this.shared.router.navigateByUrl('/');
      },
      error => {
        this.shared.toastr.error(error.error);
      }
    );
  }
  
}
