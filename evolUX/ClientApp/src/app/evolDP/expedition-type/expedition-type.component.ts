import { Component, OnInit } from '@angular/core';
import { DataService } from '../../core/services/handlers/data.service';

@Component({
  selector: 'app-expedition-type',
  templateUrl: './expedition-type.component.html',
  styleUrls: ['./expedition-type.component.scss']
})
export class ExpeditionTypeComponent implements OnInit {
  public expeditionType: any;
  constructor(public data: DataService) {}

  ngOnInit() {
    this.getExpeditionType();
  }

  getExpeditionType() {
    this.data.getExpeditionType().subscribe(result => {
      this.expeditionType = result;
      console.log(result);
    });
  }
}
