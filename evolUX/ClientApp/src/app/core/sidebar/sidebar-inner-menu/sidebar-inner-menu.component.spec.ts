import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SidebarInnerMenuComponent } from './sidebar-inner-menu.component';

describe('SidebarInnerMenuComponent', () => {
  let component: SidebarInnerMenuComponent;
  let fixture: ComponentFixture<SidebarInnerMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SidebarInnerMenuComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SidebarInnerMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
