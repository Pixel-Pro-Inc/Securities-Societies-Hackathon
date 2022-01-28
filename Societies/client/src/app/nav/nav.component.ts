import { Component, OnInit } from '@angular/core';
import { LoginService } from '../_services/loginservice.service';
import { Messages } from '../_models/messages';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  messages: Messages[];

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
