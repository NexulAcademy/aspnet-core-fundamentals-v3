import { TestBed } from '@angular/core/testing';

import { CustomerMockService } from './customer-mock.service';

describe('CustomerMockService', () => {
  let service: CustomerMockService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomerMockService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
