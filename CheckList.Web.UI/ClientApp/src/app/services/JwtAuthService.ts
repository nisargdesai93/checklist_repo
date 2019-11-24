import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class JwtAuthService {
  constructor(public jwtHelper: JwtHelperService) { }

  // Check whether the token is expired and return
  // true or false


  public isAuthenticated(): boolean {
    const token: any = this.jwtHelper.tokenGetter();
    var t = JSON.parse(token);
    return !this.jwtHelper.isTokenExpired(t.token);
  }
}
