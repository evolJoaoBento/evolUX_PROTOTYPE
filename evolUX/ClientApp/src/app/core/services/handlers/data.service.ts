import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { RemotesService } from '../providers/remotes.service';
import { AuthService } from '../providers/auth.service';

@Injectable({
  providedIn: 'root'
})
export class DataService {
  constructor(private remotes: RemotesService, private auth: AuthService) { }

  public getSidebar(): Observable<any> {
    return this.remotes.getSidebar();
  }
  public getExpeditionType(): Observable<any> {
    return this.remotes.getExpeditionType();
  }
  public getEnvelopeMedia(): Observable<any> {
    return this.remotes.getEnvelopeMedia();
  }

  /* Examples */
  public getSidebarAlt(): Observable<any> {
     return this.remotes.get("sidebar", "GetMain");
  }

  public getExpeditionZone(): Observable<any> {
    return this.remotes.get("evoldp", "ExpeditionZone");
  }
  public getResources(): Observable<any> {
    return this.remotes.get("evolflow", "Resources");
  }

  authenticate(): boolean {
    return this.auth.isAuthorized;
  }
}
