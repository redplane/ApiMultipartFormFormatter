import {NgModule, ModuleWithProviders} from '@angular/core';
import {IAccountService} from '../interfaces/services/account-service.interface';
import {AccountService} from './account.service';
import {IAuthenticationService} from '../interfaces/services/authentication-service.interface';
import {AuthenticationService} from './authentication.service';

@NgModule({})

export class ServiceModule {

  //#region Methods

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ServiceModule,
      providers: [
        {provide: 'IAccountService', useClass: AccountService},
        {provide: 'IAuthenticationService', useClass: AuthenticationService}
      ]
    };
  }

  //#endregion
}


