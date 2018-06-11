import {BasicUploadViewModel} from '../../view-models/basic-upload.view-model';
import {Observable} from 'rxjs/Rx';
import {HttpResponse} from '@angular/common/http';
import {ApiResponseViewModel} from '../../view-models/api-response.view-model';

export interface IUploadService {

  //#region Methods

  /*
  * Upload file to server.
  * */
  basicUpload(basicUploadViewModel: BasicUploadViewModel): Observable<ApiResponseViewModel>;

  //#endregion

}
