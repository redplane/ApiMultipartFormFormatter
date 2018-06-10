import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {NgModule} from "@angular/core";
import {AttachmentListUploadComponent} from './attachment-list-upload/attachment-list-upload.component';
import {BasicUploadComponent} from './basic-upload/basic-upload.component';
import {NestedInfoUploadComponent} from './nested-info-upload/nested-info-upload.component';
import {UploadRouteModule} from './upload.route';
import {UploadMasterLayoutComponent} from './master-layout/master-layout.component';
import {SharedModule} from '../shared/shared.module';

//#region Routes declaration


//#endregion

//#region Module declaration

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    UploadRouteModule,
    SharedModule
  ],
  declarations: [
    AttachmentListUploadComponent,
    BasicUploadComponent,
    NestedInfoUploadComponent,
    UploadMasterLayoutComponent
  ],
  exports: [
    AttachmentListUploadComponent,
    BasicUploadComponent,
    NestedInfoUploadComponent,
    UploadMasterLayoutComponent
  ]
})

export class UploadModule {
}

//#endregion
