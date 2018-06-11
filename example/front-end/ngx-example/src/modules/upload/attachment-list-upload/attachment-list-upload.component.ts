import {Component, Inject, ViewChild} from '@angular/core';
import {FileUploader} from 'ng2-file-upload';
import {AttachmentListUploadViewModel} from '../../../view-models/attachment-list-upload.view-model';
import {IUploadService} from '../../../interfaces/services/upload-service.interface';
import {ApiResponseViewModel} from '../../../view-models/api-response.view-model';
import {ModalDirective} from 'ngx-bootstrap';

@Component({
  selector: 'attachment-list-upload',
  templateUrl: './attachment-list-upload.component.html'
})

export class AttachmentListUploadComponent {

  //#region Properties

  // Model for information binding.
  public _oModel: AttachmentListUploadViewModel;

  // Uploader instance.
  public _attachmentUploader: FileUploader;

  // Whether attachments list is empty or not.
  public _iAttachmentCounter: number;

  // List of messages from api.
  public _messages: Array<string>;

  @ViewChild('mdApiResponse')
  public mdApiResponse: ModalDirective;

  //#endregion

  //#region Constructor

  public constructor(@Inject('IUploadService') public uploadService: IUploadService) {
    this._attachmentUploader = new FileUploader({});

    this._oModel = new AttachmentListUploadViewModel();
    this._oModel.attachments = new Array<File>();
  }

  //#endregion

  //#region Methods

  /*
  * Called when file is attached to uploader.
  * */
  public ngOnFileSelect(files: FileList): void {
    for (let iIndex = 0; iIndex < files.length; iIndex++) {
      let file = files.item(iIndex);
      this._oModel.attachments.push(file);
    }
    this._iAttachmentCounter = this._oModel.attachments.length;
  }

  /*
  * Called when attachment is deleted.
  * */
  public ngOnAttachmentDeleted(attachment: File): void {
    // Find the item index.
    let iIndex = this._oModel.attachments.indexOf(attachment);
    if (iIndex == -1)
      return;

    this._oModel.attachments.splice(iIndex, 1);
    this._iAttachmentCounter = this._oModel.attachments.length;
  }

  /*
  * Called when submit button is clicked.
  * */
  public ngOnSubmitClicked($event): void{
    if ($event)
      $event.preventDefault();

    this.uploadService.attachmentListUpload(this._oModel)
      .subscribe((apiResponse: ApiResponseViewModel) => {
        this._messages = apiResponse.messages;
        this.mdApiResponse.show();
      });
  }

  /*
  * Called when modal dialog is being hidden.
  * */
  public ngOnModalBeingHidden(): void{
    this.mdApiResponse.hide();
  }

  //#endregion
}
