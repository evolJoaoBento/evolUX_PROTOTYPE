import { Component, OnInit } from '@angular/core';

import { ApplicationControllerService } from '../services/applicationcontroller.service';

@Component({
  selector: 'app-topbar',
  templateUrl: './topbar.component.html',
  styleUrls: ['./topbar.component.scss']
})
export class TopBarComponent implements OnInit {

  constructor(public appController: ApplicationControllerService) { }

  changeLang() {
    this.appController.changeLang();
  }
  ngOnInit(): void {
  }
}
