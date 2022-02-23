import { Component, OnInit } from '@angular/core';
import temp from '../../../evolDP/sidebar.config.json';

@Component({
  selector: 'app-sidebar-inner-menu',
  templateUrl: './sidebar-inner-menu.component.html',
  styleUrls: ['./sidebar-inner-menu.component.scss']
})
export class SidebarInnerMenuComponent implements OnInit {

  constructor() { }

  public innerContent = temp;

  ngOnInit(): void {
  }
}
