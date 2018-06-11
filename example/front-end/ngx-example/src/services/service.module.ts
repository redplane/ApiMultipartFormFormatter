import {NgModule, ModuleWithProviders} from '@angular/core';
import {IAuthenticationService} from '../interfaces/services/authentication-service.interface';
import {AuthenticationService} from './authentication.service';
import {UploadService} from './upload.service';
import {HttpClientModule} from '@angular/common/http';

@NgModule({
  imports:[
    HttpClientModule
  ]
})

export class ServiceModule {

  //#region Methods

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ServiceModule,
      providers: [
        {provide: 'IUploadService', useClass: UploadService},
        {provide: 'IAuthenticationService', useClass: AuthenticationService}
      ]
    };
  }

  //#endregion
}


