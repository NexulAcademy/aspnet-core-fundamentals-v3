import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';

import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [CustomerListPageComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    MatTableModule,
    MatCardModule,
    CustomerRoutingModule
  ]
})
export class CustomerModule { }
