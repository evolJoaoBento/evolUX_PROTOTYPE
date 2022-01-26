import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-expedition-type',
  templateUrl: './expedition-type.component.html',
  styleUrls: ['./expedition-type.component.css']
})
export class ExpeditionTypeComponent implements OnInit {
  public data: any;
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<any[]>(baseUrl + 'expeditiontype').subscribe(result => {
      this.data = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }
}
