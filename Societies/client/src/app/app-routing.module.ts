import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AmmendmentsComponent } from './ammendments/ammendments.component';
import { ComplianceComponent } from './compliance/compliance.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { SignupComponent } from './signup/signup.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'signup', component: SignupComponent},
  {path: 'login', component: LoginComponent},
  { path: 'compliance', component: ComplianceComponent },
  { path: 'ammendments', component: AmmendmentsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
