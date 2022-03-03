import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-sidebar-inner-menu',
  templateUrl: './sidebar-inner-menu.component.html',
  styleUrls: ['./sidebar-inner-menu.component.scss']
})
export class SidebarInnerMenuComponent implements OnInit {
  @Input() innerContent: any;
  constructor() {}

  expanded: boolean = true;

  ngOnInit(): void {
  }

  expand() {
    this.expanded = !this.expanded
  }
}
