import {Component, OnInit} from "@angular/core";
import {ActivatedRoute} from "@angular/router";
import {ProfileViewModel} from "../../../view-models/profile.view-model";

@Component({
  selector: 'authorize-layout',
  templateUrl: 'authorize-layout.component.html'
})

export class AuthorizeLayoutComponent implements OnInit{

  //#region Properties

  /*
  * Profile information.
  * */
  private profile: ProfileViewModel;

  //#endregion

  //#region Constructor

  /*
  * Initiate component with injectors.
  * */
  public constructor(public activatedRoute: ActivatedRoute){
  }

  //#endregion

  //#region Methods

  /*
  * Event which is called when component has been initiated.
  * */
  public ngOnInit(): void {
    this.activatedRoute.data.subscribe((x: any) => {
      this.profile = <ProfileViewModel> x.profile;
    });
  }

  //#endregion
}
