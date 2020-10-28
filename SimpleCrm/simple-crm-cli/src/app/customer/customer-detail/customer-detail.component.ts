import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'crm-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.scss']
})
export class CustomerDetailComponent implements OnInit {
  customerId: number;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.customerId = this.route.snapshot.params.id;
  }
}
