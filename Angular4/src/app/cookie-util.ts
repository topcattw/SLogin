export class CookieUtil {

  setCookie(cookieName, cookieValue, cookieExpire){
    let sCookie:string='';
    sCookie += cookieName + '=' + cookieValue + ';';
    sCookie += 'expires=' + cookieExpire + ';';
    sCookie += '';

    document.cookie = sCookie;
  }

  getCookie(cname:string):string {
      var name = cname + "=";
      var decodedCookie = decodeURIComponent(document.cookie);
      var ca = decodedCookie.split(';');
      for(var i = 0; i <ca.length; i++) {
          var c = ca[i];
          while (c.charAt(0) == ' ') {
              c = c.substring(1);
          }
          if (c.indexOf(name) == 0) {
              return c.substring(name.length, c.length);
          }
      }
      return "";
  }

}
