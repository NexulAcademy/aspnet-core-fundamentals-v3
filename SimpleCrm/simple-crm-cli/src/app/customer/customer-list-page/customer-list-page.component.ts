import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { MatDialog } from '@angular/material/dialog';
import { CustomerService } from '../customer.service';
import { Observable } from 'rxjs';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { Router } from '@angular/router';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss']
})
export class CustomerListPageComponent implements OnInit {
  customers$: Observable<Customer[]>;
  displayColumns = ['icon', 'name', 'phone', 'email', 'status', 'actions'];

  constructor(
    private customerService: CustomerService,
    private router: Router,
    public dialog: MatDialog
  ) {
    this.customers$ = this.customerService.search('');
  }

  ngOnInit(): void {
  }

  openDetail(item: Customer): void {
    if (item) {
      this.router.navigate([`./customer/${item.customerId}`]);
    }
  }

  addCustomer(): void {
    const dialogRef = this.dialog.open(CustomerCreateDialogComponent, {
      width: '250px',
      data: null
    });
  }
}
