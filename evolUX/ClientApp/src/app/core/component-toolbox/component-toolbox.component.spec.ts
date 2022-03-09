import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ComponentToolboxComponent } from './component-toolbox.component';

describe('ComponentToolboxComponent', () => {
  let component: ComponentToolboxComponent;
  let fixture: ComponentFixture<ComponentToolboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ComponentToolboxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ComponentToolboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
