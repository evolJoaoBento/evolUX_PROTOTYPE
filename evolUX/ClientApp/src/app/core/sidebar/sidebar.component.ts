import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  public data: any;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<any[]>(baseUrl + 'sidebar/GetMain').subscribe(result => {
      this.data = result;
    }, error => console.error(error));
  }
  innerActive: boolean = false;
  public selectedInnerContent: any;

  ngOnInit(): void {
  }

  toggleSidebar(inner: any) {
    if (!this.selectedInnerContent) {
      this.assignInner(inner);
    } else {
      if (this.selectedInnerContent.id != inner.id) {
        this.assignInner(inner);
      } else {
        this.innerActive = !this.innerActive;
      }
    }
  }

  assignInner(_inner: any) {
    this.selectedInnerContent = _inner;
    this.innerActive = true;
  }
}
