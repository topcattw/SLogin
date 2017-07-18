import { LoginService } from './login.service';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class LoginRoutingGuard implements CanActivate {


  constructor(private svcLogin:LoginService){
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {

      //console.log(state.url);
      //取得登入後導回的網址
      this.svcLogin.backUrl = state.url;

      return this.svcLogin.isLoginedIn()
        .map((allow:boolean) => {
           // do something here
           // user.access_level;
          //return true;
          return allow;
         });          

  }
}
