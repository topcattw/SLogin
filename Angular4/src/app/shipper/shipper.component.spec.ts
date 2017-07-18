import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ShipperComponent } from './shipper.component';

describe('ShipperComponent', () => {
  let component: ShipperComponent;
  let fixture: ComponentFixture<ShipperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShipperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ShipperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
