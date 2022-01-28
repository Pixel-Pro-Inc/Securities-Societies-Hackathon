import { Component, OnInit } from '@angular/core';
import { Showingdashboard } from '../_enums/showingdashboard';
import { Application } from '../_models/application';
import { DashService } from '../_services/dash.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  show: number;
  showingdashboard: Showingdashboard;

  constructor(private dashService: DashService) { }

  ngOnInit(): void {
    //this.populateDashboard();
  }

  model: any = {};
  model1: any = {};
  model2: any = {};
  model3: any = {};
  model4: any = {};

  totalapplications: any[] = [];
  inprocessapplications: any[] = [];
  rejectedapplications: Application[];
  payments: any[] = [];

  //methods to be called where
  showTotalApplications = () => this.show=Showingdashboard.Dash;
  showInprocessApplications = () => this.show =Showingdashboard.inprocess;
  showRejectedApplications = () => this.show =Showingdashboard.rejections;
  showPaymentReport = () => this.show =Showingdashboard.Payment;
  showDashboard = () => this.show =Showingdashboard.Dash;


  generateReport(type: string) {
    if (type == 'total') {
      this.model.searched = true;

      this.dashService.getapplications(this.model, "report/totalapplication").subscribe(
        response => {
          this.totalapplications = response;

          //consider putting a ternary operator
          if (response.length == 0) {
            this.model.empty = true;
          } else {
            this.model.empty = false;
          }
        }
      )
    }
    //this.busyService.idle(); Haven't made busy service yet

    if (type == 'inprocess') {
      this.model1.searched = true;

      this.dashService.getapplications(this.model1, "report/inprocessapplication").subscribe(
        response => {
          this.inprocessapplications = response;

          //consider putting a ternary operator
          if (response.length == 0) {
            this.model1.empty = true;
          } else {
            this.model1.empty = false;
          }

        }
      )
    }

    if (type == 'rejections') {
      this.dashService.getapplications(this.model2,"report/rejectedapplication").subscribe(
        response => {
          this.rejectedapplications = response;
        }
      )
    }

    if (type == 'payment') {
      this.dashService.payment(this.model3).subscribe(
        response => {
          this.payments = response;
        }
      )
    }


  }

  expandToggle(item: any) {
    item.extra = !item.extra;
  }

  exportToExcel() {
    this.dashService.exportToExcel();
  }

}
