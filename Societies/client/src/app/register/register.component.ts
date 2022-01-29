import { Component, OnInit } from '@angular/core';
import { CallService } from '../_services/call.service';
import { SharedService } from '../_services/shared.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(public callService: CallService, public shared: SharedService) { }

  ngOnInit(): void {
  }

  isLoggedOn(){
    return this.shared.getUser() == null? false: true;
  }

}
