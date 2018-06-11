import {Component, Inject, TemplateRef, ViewChild} from '@angular/core';
import {FileUploader} from 'ng2-file-upload';
import {BasicUploadViewModel} from '../../../view-models/basic-upload.view-model';
import {User} from '../../../models/user';
import {IUploadService} from '../../../interfaces/services/upload-service.interface';
import {ApiResponseViewModel} from '../../../view-models/api-response.view-model';
import {ToastrService} from 'ngx-toastr';
import {HttpResponse} from '@angular/common/http';
import {BsModalRef, BsModalService, ModalDirective} from 'ngx-bootstrap';

@Component({
  selector: 'basic-upload',
  templateUrl: './basic-upload.component.html'
})

export class BasicUploadComponent {

  //#region Properties

  private readonly _basicFileUploader;

  // Model for information binding.
  private _oModel: BasicUploadViewModel;

  // Whether file list is empty or not.
  private _bIsFileListAvailable: boolean = false;

  // List of messages responded by api.
  private _messages: Array<string>;

  @ViewChild('mdApiResponse')
  mdApiResponse: ModalDirective;

  //#endregion


  //#region Constructor

  /*
  * Initialize component with injectors.
  * */
  public constructor(@Inject('IUploadService') public uploadService: IUploadService,
                     public bsModalService: BsModalService) {
    this._basicFileUploader = new FileUploader({});
    this._oModel = new BasicUploadViewModel();
    this._oModel.user = new User();
  }

  //#endregion

  //#region Methods

  /*
  * Called when a file is selected.
  * */
  public ngOnFileSelect(files: FileList): void {
    for (let iIndex = 0; iIndex < files.length; iIndex++) {
      let file = files[iIndex];
      this._oModel.attachment = file;
      this._bIsFileListAvailable = true;
    }
  }

  /*
  * Called when submit button is clicked.
  * */
  public ngOnSubmitClicked($event): void {

    if ($event)
      $event.preventDefault();

    this.uploadService.basicUpload(this._oModel)
      .subscribe((apiResponse: ApiResponseViewModel) => {
        this._messages = apiResponse.messages;
        this.mdApiResponse.show();
      });
  }

  //#endregion
}
