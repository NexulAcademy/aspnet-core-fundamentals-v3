import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';

import { CustomerRoutingModule } from './customer-routing.module';
import { CustomerListPageComponent } from './customer-list-page/customer-list-page.component';
import { HttpClientModule } from '@angular/common/http';
import { CustomerService } from './customer.service';
import { CustomerMockService } from './customer-mock.service';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CustomerCreateDialogComponent } from './customer-create-dialog/customer-create-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { CustomerDetailComponent } from './customer-detail/customer-detail.component';
import { StatusIconPipe } from './status-icon.pipe';

@NgModule({
  declarations: [
    CustomerListPageComponent,
    CustomerCreateDialogComponent,
    CustomerDetailComponent,
    StatusIconPipe
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatTableModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    FlexLayoutModule,
    CustomerRoutingModule
  ],
  providers: [
    {
      provide: CustomerService,
      useClass: CustomerMockService
    }
  ]
})
export class CustomerModule { }
