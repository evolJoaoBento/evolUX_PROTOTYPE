import { TestBed } from '@angular/core/testing';

import { ApplicationcontrollerService } from './applicationcontroller.service';

describe('ApplicationcontrollerService', () => {
  let service: ApplicationcontrollerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ApplicationcontrollerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
