import { Component, OnInit } from '@angular/core';
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

  constructor(
    private customerService: CustomerService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.customerId = +this.route.snapshot.params.id;
    this.customerService.get(this.customerId)
      .subscribe(cust => {
        this.customer = cust;
      });
  }
}
