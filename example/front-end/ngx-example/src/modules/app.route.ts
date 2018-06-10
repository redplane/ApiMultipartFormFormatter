import {NgModule} from '@angular/core';
import {AppComponent} from './app.component';
import {RouterModule, Routes} from '@angular/router';

//#region Properties

// Application routes configuration.
export const routes: Routes = [
  {
    path: 'upload',
    loadChildren: 'modules/upload/upload.module#UploadModule'
  },
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'upload/basic'
  }
];

//#endregion

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    RouterModule.forRoot(routes, {enableTracing: false})
  ],
  exports: [
    RouterModule
  ],
  bootstrap: [AppComponent]
})

export class AppRouteModule {
}
