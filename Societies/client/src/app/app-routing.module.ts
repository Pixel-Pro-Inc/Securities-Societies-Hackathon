import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AmmendmentsComponent } from './ammendments/ammendments.component';
import { ComplianceComponent } from './compliance/compliance.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  { path: 'register', component: RegisterComponent },
  { path: 'compliance', component: ComplianceComponent },
  { path: 'ammendments', component: AmmendmentsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
