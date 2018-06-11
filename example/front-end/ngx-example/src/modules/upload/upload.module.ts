import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {NgModule} from '@angular/core';
import {AttachmentListUploadComponent} from './attachment-list-upload/attachment-list-upload.component';
import {BasicUploadComponent} from './basic-upload/basic-upload.component';
import {NestedInfoUploadComponent} from './nested-info-upload/nested-info-upload.component';
import {UploadRouteModule} from './upload.route';
import {UploadMasterLayoutComponent} from './master-layout/master-layout.component';
import {SharedModule} from '../shared/shared.module';
import {FileUploadModule} from 'ng2-file-upload';
import {ValueValidator} from '../../validators/value.validator';
import {BsModalService, ModalModule} from 'ngx-bootstrap';

//#region Routes declaration


//#endregion

//#region Module declaration

@NgModule({
  providers: [
    BsModalService
  ],
  imports: [
    CommonModule,
    FormsModule,
    UploadRouteModule,
    SharedModule,
    FileUploadModule,
    ModalModule.forRoot()
  ],
  declarations: [
    AttachmentListUploadComponent,
    BasicUploadComponent,
    NestedInfoUploadComponent,
    UploadMasterLayoutComponent,
    ValueValidator
  ],
  exports: [
    AttachmentListUploadComponent,
    BasicUploadComponent,
    NestedInfoUploadComponent,
    UploadMasterLayoutComponent,
    ValueValidator
  ]
})

export class UploadModule {
}

//#endregion
