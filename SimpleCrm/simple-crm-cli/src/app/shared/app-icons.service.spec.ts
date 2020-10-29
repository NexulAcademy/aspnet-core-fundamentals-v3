import { TestBed } from '@angular/core/testing';

import { AppIconsService } from './app-icons.service';

describe('AppIconsService', () => {
  let service: AppIconsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AppIconsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
