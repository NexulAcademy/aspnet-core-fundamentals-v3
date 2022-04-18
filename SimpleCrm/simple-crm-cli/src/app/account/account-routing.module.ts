import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { NotAuthorizedComponent } from './not-authorized/not-authorized.component';
import { SigninMicrosoftComponent } from './signin-microsoft/signin-microsoft.component';

const routes: Routes = [
  // {
  //   path: 'register',
  //   component: RegistrationComponent
  // },
  {
    path: 'login',
    component: LoginComponent,
  },
  {
    path: 'signin-microsoft',
    component: SigninMicrosoftComponent,
  },
  // {
  //   path: 'signin-google',
  //   component: SigninGoogleComponent,
  // },
  {
    path: 'not-authorized',
    component: NotAuthorizedComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AccountRoutingModule { }
