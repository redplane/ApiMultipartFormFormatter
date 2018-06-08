import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {NgModule} from "@angular/core";
import {AuthorizeLayoutComponent} from "./authorize-layout/authorize-layout.component";
import {NavigationBarComponent} from "./navigation-bar/navigation-bar.component";
import {SideBarComponent} from "./side-bar/side-bar.component";
import {RouterModule} from "@angular/router";
import {MomentModule} from 'ngx-moment';

//#region Module declaration

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    MomentModule,
    RouterModule
  ],
  declarations: [
    AuthorizeLayoutComponent,
    NavigationBarComponent,
    SideBarComponent
  ]
})

export class SharedModule {
}

//#endregion
