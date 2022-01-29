import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AmmendmentsComponent } from './ammendments/ammendments.component';
import { ComplianceComponent } from './compliance/compliance.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { MyapplicationsComponent } from './myapplications/myapplications.component';
import { NameauthComponent } from './nameauth/nameauth.component';
import { RegisterComponent } from './register/register.component';
import { SignupComponent } from './signup/signup.component';
import { LoggedInGuard } from './_guards/logged-in.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'signup', component: SignupComponent},
  {path: 'login', component: LoginComponent},
  { path: 'compliance', component: ComplianceComponent },
  { path: 'ammendments', component: AmmendmentsComponent },
  { path: 'nameauthorisation', component: NameauthComponent, canActivate: [LoggedInGuard] },
  { path: 'myapplications', component: MyapplicationsComponent, canActivate: [LoggedInGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
