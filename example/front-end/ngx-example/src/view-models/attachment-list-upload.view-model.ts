import {User} from '../models/user';

export class AttachmentListUploadViewModel{

  //#region Properties

  // User instance.
  public user: User;

  // List of attachments to be uploaded.
  public attachments: Array<File>;

  //#endregion

  //#region Constructor

  public constructor(){
    this.user = new User();
    this.attachments = new Array<File>();
  }

  //#endregion
}
