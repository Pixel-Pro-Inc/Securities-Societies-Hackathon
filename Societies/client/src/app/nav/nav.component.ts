import { Component, OnInit } from '@angular/core';
import { LoginService } from '../_services/loginservice.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private loginService: LoginService) { }

  ngOnInit(): void {
  }

  loggedIn(): boolean {
    if (localStorage.getItem('user') != null) {
      return true;
    }
    return false;
  }
  /**
   * getUser(): User {
    let userString = '';
    userString = (String)(localStorage.getItem('user'));

    let user: User = JSON.parse(userString);

    return user;
  }
   * */
 

  logout() {
    this.loginService.logout();
  }
}
