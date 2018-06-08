/**
 * Created by Linh Nguyen on 6/7/2017.
 */
import {Inject, Injectable} from "@angular/core";
import {CanActivate, Router} from "@angular/router";
import {IAuthenticationService} from "../interfaces/services/authentication-service.interface";

@Injectable()
export class IsAuthorizedGuard implements CanActivate {

  //#region Constructor
  /*
  * Initiate guard component with injectors.
  * */
  public constructor(
    @Inject('IAuthenticationService') private authenticationService: IAuthenticationService,
    private router: Router) {}

  //#endregion

  //#region Methods

  /*
  * Check whether route can be activated or not.
  * */
  public canActivate(): boolean {


    // Find identity stored in cache.
    let identity = this.authenticationService.getAuthorization();

    // No identity has been found.
    if (!this.authenticationService.isAuthorizationValid(identity)) {
      // Redirect to login.
      this.router.navigate(['/login']);
      return false;
    }

    return true;
  }

  //#endregion
}
