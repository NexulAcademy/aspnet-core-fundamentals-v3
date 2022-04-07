import { Component, OnInit } from '@angular/core';
import { Customer } from '../customer.model';
import { MatDialog } from '@angular/material/dialog';
import { CustomerService } from '../customer.service';
import { combineLatest, Observable } from 'rxjs';
import { debounceTime, map, startWith, switchMap } from 'rxjs/operators';
import { CustomerCreateDialogComponent } from '../customer-create-dialog/customer-create-dialog.component';
import { Router } from '@angular/router';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'crm-customer-list-page',
  templateUrl: './customer-list-page.component.html',
  styleUrls: ['./customer-list-page.component.scss']
})
export class CustomerListPageComponent implements OnInit {
  filteredCustomers$: Observable<Customer[]>;
  displayColumns = ['icon', 'name', 'phone', 'email', 'status', 'actions'];
  filterInput = new FormControl();

  constructor(
    private customerService: CustomerService,
    private router: Router,
    public dialog: MatDialog
  ) {
    this.filteredCustomers$ = this.filterInput.valueChanges.pipe(
      startWith(''),
      debounceTime(700),
      switchMap((filterTerm: string) => {
        return this.customerService.search(filterTerm);
      }),
    );
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
