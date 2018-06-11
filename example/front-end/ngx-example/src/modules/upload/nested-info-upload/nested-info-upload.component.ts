import {Component, Inject, ViewChild} from '@angular/core';
import {FileUploader} from 'ng2-file-upload';
import {IUploadService} from '../../../interfaces/services/upload-service.interface';
import {ProfileViewModel} from '../../../view-models/profile.view-model';
import {NestedInfoUploadViewModel} from '../../../view-models/nested-info-upload.view-model';
import {ApiResponseViewModel} from '../../../view-models/api-response.view-model';
import {ModalDirective} from 'ngx-bootstrap';

@Component({
  selector: 'nested-info-upload',
  templateUrl: './nested-info-upload.component.html'
})

export class NestedInfoUploadComponent {

  //#region Properties

  public _attachmentUploader: FileUploader;

  public _profileAttachmentUploader: FileUploader;

  public _oModel: NestedInfoUploadViewModel;

  public _iAttachmentCounter: number;

  // List of messages from api.
  public _messages: Array<string>;

  @ViewChild('mdApiResponse')
  public mdApiResponse: ModalDirective;

  //#endregion

  //#region Constructor

  /*
  * Initialize component with injectors.
  * */
  public constructor(@Inject('IUploadService') public uploadService: IUploadService) {
    this._attachmentUploader = new FileUploader({});
    this._profileAttachmentUploader = new FileUploader({});
    this._oModel = new NestedInfoUploadViewModel();
    this._oModel.profile = new ProfileViewModel();
  }

  //#endregion

  //#region Methods

  /*
  * Called when an attachment is attached.
  * */
  public ngOnAttachmentSelected(fileList: FileList) {
    for (let iIndex = 0; iIndex < fileList.length; iIndex++) {
      this._oModel.attachment = fileList.item(iIndex);
    }

    this._iAttachmentCounter = 1;
  }

  /*
  * Called when a profile attachment is attached.
  * */
  public ngOnProfileAttachmentSelected(fileList: FileList) {
    for (let iIndex = 0; iIndex < fileList.length; iIndex++) {
      this._oModel.profile.attachment = fileList.item(iIndex);
    }
  }

  /*
  * Called when submit button is clicked.
  * */
  public ngOnSubmitClicked(): void {
    this.uploadService.nestedInfoUpload(this._oModel)
      .subscribe((apiResponseMessage: ApiResponseViewModel) => {
        let messages = apiResponseMessage.messages;
        this._messages = messages;

        this.mdApiResponse.show();
      });
  }

  public ngOnModalBeingHidden(): void {
    this.mdApiResponse.hide();
  }

  //#endregion

}
