import { Component, OnInit } from '@angular/core';
import { MasterService } from '../_services/master.service';

@Component({
  selector: 'app-compliance',
  templateUrl: './compliance.component.html',
  styleUrls: ['./compliance.component.css']
})
export class ComplianceComponent implements OnInit {
  constructor(private master: MasterService) { }

  ngOnInit(): void {
  }
  siteTelephone = this.master.siteTelephone;
}
