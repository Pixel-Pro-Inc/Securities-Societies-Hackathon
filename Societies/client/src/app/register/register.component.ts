import { Component, OnInit } from '@angular/core';
import { CallService } from '../_services/call.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(public callService: CallService) { }

  ngOnInit(): void {
  }

}
