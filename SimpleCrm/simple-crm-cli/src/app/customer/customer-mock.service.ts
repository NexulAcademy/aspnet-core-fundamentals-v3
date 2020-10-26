import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Customer } from './customer.model';
import { CustomerService } from './customer.service';

@Injectable()
export class CustomerMockService extends CustomerService {
  customers: Customer[] = [];
  lastCustomerId: number;

  constructor(http: HttpClient) {
    super(http);
    console.warn('Warning: You are using the CustomerMockService, not intended for production use.');

    const localCustomers = localStorage.getItem('customers');
    if (localCustomers) {
      this.customers = JSON.parse(localCustomers);
    } else {
      this.customers.push({
        customerId: 1,
        firstName: 'Tory',
        lastName: 'Amos',
        phoneNumber: '314-555-9873',
        emailAddress: 'tory@example.com',
        statusCode: 'Prospect',
        preferredContactMethod: 'email',
        lastContactDate: new Date().toISOString()
      });
    }
    this.lastCustomerId = Math.max(...this.customers.map(x => x.customerId));
  }
  search(term: string): Observable<Customer[]> {
    const items = this.customers.filter(x =>
      (x.firstName + ' ' + x.lastName).indexOf(term) >= 0
      || x.phoneNumber.indexOf(term) >= 0
      || x.emailAddress.indexOf(term) >= 0);
    // convert the array into an observable of the array to meet the function signature
    return of(items);
  }

  insert(customer: Customer): Observable<Customer> {
    customer.customerId = Math.max(...this.customers.map(x => x.customerId));
    this.customers = [...this.customers, customer];
    localStorage.setItem('customers', JSON.stringify(this.customers));
    // convert a result instance into an observable of the array to meet the function signature
    return of(customer);
  }

  update(customer: Customer): Observable<Customer> {
    const match = this.customers.find(x => x.customerId === customer.customerId);
    if (match) {
      // replace the matched item, keep other items unchanged
      this.customers = this.customers
        .map(x => x.customerId === customer.customerId ? customer : x);
    } else { // not in the list yet
      this.customers = [...this.customers, customer];
    }
    localStorage.setItem('customers', JSON.stringify(this.customers));
    // convert a result instance into an observable of the array to meet the function signature
    return of(customer);
  }
}
