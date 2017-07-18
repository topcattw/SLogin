import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Util } from './util';
import { CookieUtil } from './cookie-util';
import { Router } from '@angular/router';
import { Http, RequestOptions, Headers, Response } from '@angular/http';

@Injectable()
export class LoginService {
  oUtil = new Util();
  oUtilCook = new CookieUtil();
  error:any;

  backUrl:string='';

  oLoginIn:any={UsrID:'',PW:''};
  oLoginOut:any={};
  oUser:any={};

  constructor(private http:Http,private router:Router) { }


  //進行登入驗證
  doLogin(){
    let url=this.oUtil.serverUrl +'/api/User/Login';
    let headers = new Headers({'Content-Type':'application/json'});
    let options = new RequestOptions({headers:headers});
    console.log(this.oLoginIn)
    this.http.post(url,this.oLoginIn, options)
    .subscribe(
      (value:Response)=>{
        this.oLoginOut = value.json();
        console.log(this.oLoginOut);
        if(this.oLoginOut.UToken!=''){
          console.log('backUrl:' + this.backUrl);
          
          //將UToken放入Cookie中
          this.UTokenSetCookie(this.oLoginOut.UToken);
          this.router.navigate([this.backUrl]);
        }
      },
      (error)=>{
        console.log(error.json());
        this.error = error.json();
      }
    );
  }

//判斷是否已經登入
isLoginedIn():Observable<boolean>{
  // let UToken:string=this.getCookie("UToken");
  let url=this.oUtil.serverUrl +'/api/User/UToken/';
  let headers = new Headers({'Content-Type':'application/json'});

  //從Cookie取得UToken
  let UToken:string=this.oUtilCook.getCookie('UToken');
  //console.log('UTokenFromCookie:' + UToken);
  if(UToken==''){
    UToken=this.oLoginOut.UToken;
    //console.log('UTokenFromLoginService:' + UToken);
  }
  //將UToken放入Header中
  headers.append('UToken',UToken);
  let options = new RequestOptions({headers:headers});
  return this.http.get(url, options)
    .map(() => {
      //成功，重新將UToken存入Cookie
      this.UTokenSetCookie(UToken);
      return true;
    })
    .catch(() => {
      // this is executed on a 401 or on any error
      //失敗，導入登入畫面。
      this.router.navigate(['/login']);
      return Observable.of(false);
    });
  }

  //傳入UToken，將UToken存入Cookie中
  UTokenSetCookie(UToken:string){
    this.oUtilCook.setCookie('UToken',UToken,20*60*1000);
  }

}
