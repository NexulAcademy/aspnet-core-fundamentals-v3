import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AccountRoutingModule } from './account-routing.module';
import { NotAuthorizedComponent } from './not-authorized/not-authorized.component';
import { MatIconModule } from '@angular/material/icon';


@NgModule({
  declarations: [NotAuthorizedComponent],
  imports: [
    CommonModule,
    AccountRoutingModule,
    MatIconModule,
  ]
})
export class AccountModule { }
