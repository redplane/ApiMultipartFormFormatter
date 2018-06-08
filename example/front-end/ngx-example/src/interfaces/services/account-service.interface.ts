import {ProfileViewModel} from "../../view-models/profile.view-model";
import {Observable} from "rxjs/Observable";

export interface IAccountService {

  //#region Methods

  /*
  * Get profile information.
  * */
  getProfile(): Observable<ProfileViewModel>;

  //#endregion

}
