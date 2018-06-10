import {Component, OnInit, Renderer2} from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/mergeMap';

@Component({
  selector: 'body',
  templateUrl: 'app.component.html'
})

export class AppComponent implements OnInit {

  title = 'app';

  //#region Constructor

  public constructor(private router: Router,
                     private activatedRoute: ActivatedRoute,
                     private renderer: Renderer2) {

  }

  //#endrgion

  //#region Methods

  /*
  * Called when component is being initiated.
  * */
  public ngOnInit(): void {
  }


  //#endregion
}
