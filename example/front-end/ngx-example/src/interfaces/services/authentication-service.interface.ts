/**
 * Created by Linh Nguyen on 6/7/2017.
 */
import {AuthorizationToken} from "../../models/authorization-token";

export interface IAuthenticationService {

  //#region Methods

  /*
   * Save identity into local storage.
   * */
  setAuthorization(identity: AuthorizationToken): void;

  /*
   * Get identity in local storage.
   * */
  getAuthorization(): AuthorizationToken;

  /*
   * Remove identity from cache.
   * */
  clearIdentity(): void;

  /*
   * Get authorization token from local storage.
   * */
  getAuthorization(): AuthorizationToken;

  /*
   * Check whether authorization token is valid or not.
   * */
  isAuthorizationValid(authorizationToken: AuthorizationToken): boolean;

  /*
  * Redirect to login page.
  * */
  redirectToLogin(): void;

//#endregion
}
