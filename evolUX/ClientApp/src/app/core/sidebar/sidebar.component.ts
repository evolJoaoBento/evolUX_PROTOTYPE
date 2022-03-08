import { Component, OnInit } from '@angular/core';

import { DataService } from '../services/handlers/data.service';
import { ApplicationControllerService } from '../services/applicationcontroller.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {

  constructor(public data: DataService, public appController: ApplicationControllerService) {}
  innerActive: boolean = false;
  innerContent: any;
  public selectedInnerContent: any;

  ngOnInit(): void {
    this.getSidebar();
  }

  getSidebar() {
    this.data.getSidebar().subscribe(result => {
      this.innerContent = result;
    });
  }

  toggleSidebar(inner: any) {
    if (!this.selectedInnerContent) {
      this.assignInner(inner);
    } else {
      if (this.selectedInnerContent.id != inner.id) {
        this.assignInner(inner);
      } else {
        this.resetInner();
      }
    }
  }
  assignInner(_inner: any) {
    this.selectedInnerContent = _inner;
    this.innerActive = true;
    this.appController.assignInnerSidebar(this.selectedInnerContent);
  }
  resetInner() {
    this.selectedInnerContent = null;
    this.innerActive = false;
    this.appController.assignInnerSidebar();
  }
}
