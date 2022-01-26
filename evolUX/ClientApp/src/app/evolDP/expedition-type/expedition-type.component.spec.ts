import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpeditionTypeComponent } from './expedition-type.component';

describe('ExpeditionTypeComponent', () => {
  let component: ExpeditionTypeComponent;
  let fixture: ComponentFixture<ExpeditionTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExpeditionTypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpeditionTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
