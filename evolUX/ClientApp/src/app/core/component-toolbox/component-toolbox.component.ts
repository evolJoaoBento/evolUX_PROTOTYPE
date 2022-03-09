import { Component, OnInit } from '@angular/core';
import { NavigationPath } from './navigation-path';

@Component({
  selector: 'app-component-toolbox',
  templateUrl: './component-toolbox.component.html',
  styleUrls: ['./component-toolbox.component.scss']
})
export class ComponentToolboxComponent implements OnInit {

  constructor() { }

  test: NavigationPath[] = [];

  ngOnInit(): void {
    this.buildTest();
  }

  buildTest() {
    const t: NavigationPath = new NavigationPath("Component", "Router");
    const _t: NavigationPath = new NavigationPath("Inner", "Router");
    const __t: NavigationPath = new NavigationPath("InnerInner", "Router");
    this.test.push(t);
    this.test.push(_t);
    this.test.push(__t);
  }
}
