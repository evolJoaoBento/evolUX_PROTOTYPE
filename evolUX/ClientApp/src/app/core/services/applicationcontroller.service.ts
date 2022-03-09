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

  //sidebar
  activeSidebarId: number = 0;

  theme: boolean = true;
  defaultTheme: string = "es-theme";
  public currentTheme: string = this.defaultTheme;

  public assignInnerSidebar(inner?: any) {
    if (inner) {
      this.assignInnerSidebarId(inner.id);
      if (this.theme) {
        this.assignTheme(inner.theme);
      }
    } else {
      this.assignInnerSidebarId(0);
      if (this.theme) {
        this.assignTheme(this.defaultTheme);
      }
    }
  }
  assignInnerSidebarId(id: number) {
    this.activeSidebarId = id;
  }
  assignTheme(theme: string = "") {
    if (theme) {
      this.currentTheme = theme;
    } else {
      this.currentTheme = this.defaultTheme;
    }
  }
}
