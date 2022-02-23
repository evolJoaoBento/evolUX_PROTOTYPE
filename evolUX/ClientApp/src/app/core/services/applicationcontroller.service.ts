import { Injectable } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';

@Injectable({
  providedIn: 'root'
})
export class ApplicationControllerService {

  constructor(public translate: TranslateService) { }

  //language
  pt: boolean = false;
  public changeLang() {
    if (this.pt) {
      this.translate.use('en');
    } else {
      this.translate.use('pt');
    }
    this.pt = !this.pt;
  }

  
}
