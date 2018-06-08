import {NgModule, ModuleWithProviders} from '@angular/core';
import {IsAuthorizedGuard} from './is-authorized-guard';

@NgModule()

export class GuardModule {

  static forRoot(): ModuleWithProviders {
    return {
      ngModule: GuardModule,
      providers: [
        IsAuthorizedGuard
      ]
    };
  }
}
