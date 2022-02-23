import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';

//ng-material
import { MatTooltipModule } from '@angular/material/tooltip';

//ngx-translate
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

//app
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';

//core
import { TopBarComponent } from './core/topbar/topbar.component';
import { SidebarComponent } from './core/sidebar/sidebar.component';
import { FooterComponent } from './core/footer/footer.component';
import { HelpComponent } from './core/help/help.component';
import { LoginComponent } from './core/login/login.component';

//evolDP
import { ExpeditionTypeComponent } from './evolDP/expedition-type/expedition-type.component';

import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { SidebarInnerMenuComponent } from './core/sidebar/sidebar-inner-menu/sidebar-inner-menu.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ExpeditionTypeComponent,
    TopBarComponent,
    SidebarComponent,
    FooterComponent,
    HelpComponent,
    LoginComponent,
    SidebarInnerMenuComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient],
      },
      defaultLanguage: 'en'
    }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'exp', component: ExpeditionTypeComponent },
    ]),
    NoopAnimationsModule,
    MatTooltipModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
