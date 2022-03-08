import { Component, OnInit } from '@angular/core';
import { DataService } from '../../core/services/handlers/data.service';

@Component({
  selector: 'app-envelope-media',
  templateUrl: './envelope-media.component.html',
  styleUrls: ['./envelope-media.component.scss']
})
export class EnvelopeMediaComponent implements OnInit {
  public envelopeMedia: any;
  constructor(public data: DataService) { }

  ngOnInit(): void {
    this.getEnvelopeMedia();
  }

  getEnvelopeMedia() {
    this.data.getEnvelopeMedia().subscribe(result => {
      this.envelopeMedia = result;
      console.log(result);
    });
  }
}
