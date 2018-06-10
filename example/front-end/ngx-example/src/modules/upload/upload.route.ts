import {RouterModule, Routes} from '@angular/router';
import {NgModule} from '@angular/core';
import {BasicUploadComponent} from './basic-upload/basic-upload.component';
import {AttachmentListUploadComponent} from './attachment-list-upload/attachment-list-upload.component';
import {NestedInfoUploadComponent} from './nested-info-upload/nested-info-upload.component';
import {UploadMasterLayoutComponent} from './master-layout/master-layout.component';

//#region Route configuration

const routes: Routes = [
  {
    path: '',
    component: UploadMasterLayoutComponent,
    children: [
      {
        path: 'basic',
        component: BasicUploadComponent,
        pathMatch: 'full'
      },
      {
        path: 'attachment-list',
        component: AttachmentListUploadComponent,
        pathMatch: 'full'
      },
      {
        path: 'nested-info',
        component: NestedInfoUploadComponent,
        pathMatch: 'full'
      },
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'basic'
      },
    ]
  }
];


//#endregion

//#region Module configuration

@NgModule({
  imports: [
    RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class UploadRouteModule {
}

//#endregion
