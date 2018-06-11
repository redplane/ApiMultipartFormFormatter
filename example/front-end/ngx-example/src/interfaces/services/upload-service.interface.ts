import {BasicUploadViewModel} from '../../view-models/basic-upload.view-model';
import {Observable} from 'rxjs/Rx';
import {HttpResponse} from '@angular/common/http';
import {ApiResponseViewModel} from '../../view-models/api-response.view-model';
import {AttachmentListUploadViewModel} from '../../view-models/attachment-list-upload.view-model';
import {NestedInfoUploadViewModel} from '../../view-models/nested-info-upload.view-model';

export interface IUploadService {

  //#region Methods

  /*
  * Upload file to server.
  * */
  basicUpload(basicUploadViewModel: BasicUploadViewModel): Observable<ApiResponseViewModel>;

  /*
  * Upload attachments list with information to back-end service.
  * */
  attachmentListUpload(attachmentListUpload: AttachmentListUploadViewModel): Observable<ApiResponseViewModel>;

  /*
  * Upload nested info to api end-point.
  * */
  nestedInfoUpload(nestedInfoUpload: NestedInfoUploadViewModel): Observable<ApiResponseViewModel>;

  //#endregion

}
