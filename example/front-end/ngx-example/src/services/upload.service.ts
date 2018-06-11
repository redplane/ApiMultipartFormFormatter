import {IUploadService} from '../interfaces/services/upload-service.interface';
import {BasicUploadViewModel} from '../view-models/basic-upload.view-model';
import {environment} from '../environments/environment';
import {HttpClient, HttpHeaders, HttpResponse} from '@angular/common/http';
import {Observable} from 'rxjs/Rx';
import {Injectable} from '@angular/core';
import {ApiResponseViewModel} from '../view-models/api-response.view-model';
import {AttachmentListUploadViewModel} from '../view-models/attachment-list-upload.view-model';
import {NestedInfoUploadViewModel} from '../view-models/nested-info-upload.view-model';

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

  /*
  * Upload attachments list with information to back-end service.
  * */
  public attachmentListUpload(attachmentListUpload: AttachmentListUploadViewModel): Observable<ApiResponseViewModel> {
    const fullUrl = `${environment.baseUrl}/api/upload/attachments-list-upload`;

    let user = attachmentListUpload.user;
    let oFormData = new FormData();
    oFormData.append('author[fullName]', user.fullName);

    let attachments = attachmentListUpload.attachments;
    for (let iIndex = 0; iIndex < attachments.length; iIndex++)
      oFormData.append(`attachments[${iIndex}]`, attachments[iIndex]);

    return this
      .httpClient
      .post<ApiResponseViewModel>(fullUrl, oFormData);
  }

  /*
  * Upload nested info to api end-point.
  * */
  public nestedInfoUpload(nestedInfoUpload: NestedInfoUploadViewModel): Observable<ApiResponseViewModel> {
    const fullUrl = `${environment.baseUrl}/api/upload/nested-info-upload`;

    let oFormData = new FormData();
    oFormData.append('attachment', nestedInfoUpload.attachment);

    let profile = nestedInfoUpload.profile;
    if (profile != null){
      if (profile.name)
          oFormData.append('profile[name]', profile.name);

      if (profile.attachment)
          oFormData.append('profile[attachment]', profile.attachment);
    }
    return this
      .httpClient
      .post<ApiResponseViewModel>(fullUrl, oFormData);
  }

  //#endregion
}
