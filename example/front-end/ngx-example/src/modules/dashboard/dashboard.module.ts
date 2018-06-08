import {DashboardComponent} from "./dashboard.component";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {NgModule} from "@angular/core";
import {DashboardRouteModule} from "./dashboard.route";
import {SharedModule} from "../shared/shared.module";

//#region Routes declaration


//#endregion

//#region Module declaration

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SharedModule,
    DashboardRouteModule
  ],
  declarations: [
    DashboardComponent
  ],
  exports: [
    DashboardComponent
  ]
})

export class DashboardModule {
}

//#endregion
