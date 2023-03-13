import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnvelopeMediaComponent } from './envelope-media.component';

describe('EnvelopeMediaComponent', () => {
  let component: EnvelopeMediaComponent;
  let fixture: ComponentFixture<EnvelopeMediaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EnvelopeMediaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EnvelopeMediaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
