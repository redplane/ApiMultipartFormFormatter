import {User} from '../models/user';

export class BasicUploadViewModel {
  //#region Properties

  /*
  * User information.
  * */
  public user: User;

  /*
  * Attachment to upload.
  * */
  public attachment: File;

  //#endregion

  //#region Constructor

  public constructor(){
    this.user = new User();
  }

  //#endregion
}
