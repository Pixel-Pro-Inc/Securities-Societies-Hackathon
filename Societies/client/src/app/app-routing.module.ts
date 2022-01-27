import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ComplianceComponent } from './compliance/compliance.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  { path: 'register', component: RegisterComponent },
  { path: 'compliance', component: ComplianceComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
