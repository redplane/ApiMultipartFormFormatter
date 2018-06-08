import {Component, Inject, ViewChild} from "@angular/core";
import {LoginViewModel} from "../../../view-models/login.view-model";
import {IAuthenticationService} from "../../../interfaces/services/authentication-service.interface";
import {AuthorizationToken} from "../../../models/authorization-token";
import {Router} from "@angular/router";

@Component({
  selector: 'account-login',
  templateUrl: 'login.component.html'
})

export class LoginComponent {

  //#region Properties

  /*
  * Model for 2-way data binding.
  * */
  private model: LoginViewModel;

  /*
  * Whether component is busy or not.
  * */
  private bIsBusy: boolean;

  //#endregion

  //#region Constructor

  public constructor(@Inject('IAuthenticationService') public authenticationService: IAuthenticationService,
                     public router: Router) {
    this.model = new LoginViewModel();

  }

  //#endregion

  //#region Methods

  /*
  * Callback which is fired when login button is clicked.
  * */
  public clickLogin($event): void {

    // Prevent default behaviour.
    $event.preventDefault();

    // Generate a forgery token and set to local storage.
    let authorizationToken = new AuthorizationToken();
    authorizationToken.code = '12345';
    authorizationToken.expire = new Date().getTime() + 3600000;
    authorizationToken.lifeTime = 3600;
    this.authenticationService.setAuthorization(authorizationToken);

    // Redirect to dashboard.
    this.router.navigate(['/dashboard']);
  }

  //#endregion
}
