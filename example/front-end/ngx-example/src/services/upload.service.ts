import {IUploadService} from '../interfaces/services/upload-service.interface';
import {BasicUploadViewModel} from '../view-models/basic-upload.view-model';
import {environment} from '../environments/environment';
import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import {Observable} from 'rxjs/Rx';
import {Injectable} from '@angular/core';
import {ApiResponseViewModel} from '../view-models/api-response.view-model';

@Injectable()
export class UploadService implements IUploadService {

  //#region Constructor

  /*
  * Initialize service with injectors.
  * */
  public constructor(public httpClient: HttpClient) {

  }

  //#endregion

  //#region Methods

  /*
  * Upload file to service end-point.
  * */
  public basicUpload(basicUploadViewModel: BasicUploadViewModel): Observable<ApiResponseViewModel> {
    const fullUrl = `${environment.baseUrl}/api/upload/basic-upload`;

    let user = basicUploadViewModel.user;
    let oFormData = new FormData();
    oFormData.append('author[fullName]', user.fullName);
    oFormData.append('attachment', basicUploadViewModel.attachment);

    return this
      .httpClient
      .post<ApiResponseViewModel>(fullUrl, oFormData);
  }

  //#endregion
}
