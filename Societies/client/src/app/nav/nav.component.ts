import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { SharedService } from '../_services/shared.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private accountService: AccountService, private shared: SharedService) { }

  ngOnInit(): void {
  }

  loggedIn(): boolean {
    if (this.shared.getUser() != null) {
      return true;
    }
    return false;
  } 

  logout() {
    this.accountService.logout();
  }
}
