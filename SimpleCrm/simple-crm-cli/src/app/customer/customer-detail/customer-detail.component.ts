import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { Customer } from '../customer.model';
import { CustomerService } from '../customer.service';

@Component({
  selector: 'crm-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss']
})
export class CustomerDetailComponent implements OnInit {
  customerId: number;
  customer: Customer;
  detailForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute
  ) {
    this.createForm();
  }

  createForm(): void {
    this.detailForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      phoneNumber: [''],
      emailAddress: ['', [Validators.required, Validators.email]],
      preferredContactMethod: ['email']
    });
  }

  ngOnInit(): void {
    this.customerId = +this.route.snapshot.params.id;
    this.customerService.get(this.customerId)
      .subscribe(cust => {
        this.customer = cust;
        this.detailForm.patchValue(cust);
      });
  }

  save(): void {
    if (!this.detailForm.valid) { return; }
    const customer = { ...this.customer, ...this.detailForm.value };
    this.customerService.update(customer)
      .subscribe(result => {
        if (!result) {
          this.snackBar.open('Error updating customer record.', 'OOPS');
          return;
        }
        this.snackBar.open('Customer updated', 'OK');
      });
  }
}
