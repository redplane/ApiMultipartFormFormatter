/**
 * Created by Linh Nguyen on 6/17/2017.
 */
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';
import {Injectable} from "@angular/core";
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs/Observable";

@Injectable()
export class GlobalHttpInterceptor implements HttpInterceptor {

  //#region Constructor

  public constructor() {
  }

  //#endregion

  //#region Methods

  /*
  * Callback which is raised when interceptor starts intercepting request.
  * */
  public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    console.log("intercepted request ... ");

    // Clone the request to add the new header.
    const authReq = req.clone({headers: req.headers.set("headerName", "headerValue")});

    console.log("Sending request with new header now ...");

    //send the newly created request
    return next.handle(authReq) as any;
  }

  //#endregion
}
