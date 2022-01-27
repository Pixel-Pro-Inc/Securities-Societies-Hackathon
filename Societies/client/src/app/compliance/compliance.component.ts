import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-compliance',
  templateUrl: './compliance.component.html',
  styleUrls: ['./compliance.component.css']
})
export class ComplianceComponent implements OnInit {
  siteTelephone: string;
  constructor() { }

  ngOnInit(): void {
  }

}
