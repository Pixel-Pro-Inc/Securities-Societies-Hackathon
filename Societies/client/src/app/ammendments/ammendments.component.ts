import { Component, OnInit } from '@angular/core';
import { MasterService } from '../_services/master.service';

@Component({
  selector: 'app-ammendments',
  templateUrl: './ammendments.component.html',
  styleUrls: ['./ammendments.component.css']
})
export class AmmendmentsComponent implements OnInit {

  constructor(private master:MasterService) { }

  ngOnInit(): void {
  }
  siteTelephone = this.master.siteTelephone;
}
