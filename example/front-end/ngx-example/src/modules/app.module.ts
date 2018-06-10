import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {FormsModule} from '@angular/forms';
import {MomentModule} from 'ngx-moment';
import {AppRouteModule} from './app.route';
import {AppSettings} from '../constants/app-settings.constant';
import {ServiceModule} from '../services/service.module';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {HttpClient, HttpClientModule} from '@angular/common/http';
import {HttpLoaderFactory} from '../factories/ngx-translate.factory';
import {GuardModule} from '../guards/guard.module';
import {SharedModule} from './shared/shared.module';

//#region Module declaration

@NgModule({
  declarations: [],
  imports: [
    FormsModule,
    BrowserModule,
    BrowserAnimationsModule, // required animations module
    MomentModule,
    HttpClientModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),

    // Application modules.
    GuardModule.forRoot(),
    ServiceModule.forRoot(),

    AppRouteModule

  ],
  providers: [
    AppSettings
  ],
  bootstrap: [AppComponent]
})


export class AppModule {
}

//#endregion
