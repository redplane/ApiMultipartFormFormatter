import {ProfileViewModel} from '../view-models/profile.view-model';
import {ActivatedRouteSnapshot, Resolve, RouterStateSnapshot} from '@angular/router';
import {Observable} from 'rxjs/Observable';
import {Inject, Injectable} from '@angular/core';
import {IAccountService} from '../interfaces/services/account-service.interface';

@Injectable()
export class ProfileResolve implements Resolve<ProfileViewModel> {

  //#region Constructors

  /*
  * Initialize resolve with injectors.
  * */
  public constructor(@Inject('IAccountService') public accountService: IAccountService) {

  }

  //#endregion

  //#region Methods

  /*
  * Resolve route data.
  * */
  public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<ProfileViewModel> | Promise<ProfileViewModel> | ProfileViewModel {
    return this.accountService.getProfile();
  }

  //#endregion

}
