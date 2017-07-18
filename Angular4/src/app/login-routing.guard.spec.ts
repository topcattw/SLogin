import { TestBed, async, inject } from '@angular/core/testing';

import { LoginRoutingGuard } from './login-routing.guard';

describe('LoginRoutingGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LoginRoutingGuard]
    });
  });

  it('should ...', inject([LoginRoutingGuard], (guard: LoginRoutingGuard) => {
    expect(guard).toBeTruthy();
  }));
});
