import {Component, Inject, Input} from '@angular/core';
import {Router} from "@angular/router";
import {IAuthenticationService} from "../../../interfaces/services/authentication-service.interface";
import {ProfileViewModel} from "../../../view-models/profile.view-model";

@Component({
  selector: 'navigation-bar',
  templateUrl: 'navigation-bar.component.html'
})

export class NavigationBarComponent {

  //#region Properties

  // Account property.
  @Input('profile')
  private profile: ProfileViewModel;

  //#endregion

  //#region Constructor

  // Initiate instance with IoC.
  public constructor(@Inject("IAuthenticationService") public authenticationService: IAuthenticationService,
                     public router: Router) {
  }

  //#endregion

  //#region Methods

  /*
  * Sign the user out.
  * */
  public clickSignOut(): void {
    // Clear the authentication service.
    this.authenticationService.clearIdentity();

    // Re-direct to login page.
    this.router.navigate(['/login']);
  }

  //#endregion
}
