import { Component } from '@angular/core'
import { UserAuthenticationEditModel } from '../Model/user.authentication.edit.model';
import { HttpWrapper } from '../services/http.wrapper.service';
import { Console } from '@angular/core/src/console';
import { UserRegistrationEditModel } from '../Model/user.registration.edti.model';
import { Router, ActivatedRoute } from '@angular/router';
import { UserSessionModel } from '../Model/user.session.model';
import decode from 'jwt-decode';


@Component({
  selector: 'user-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})

export class LoginComponent {

  authenticationModel: UserAuthenticationEditModel = new UserAuthenticationEditModel();
  registrationModel: UserRegistrationEditModel = new UserRegistrationEditModel();
  userSessionModel: UserSessionModel = new UserSessionModel();

  tokenPayload: any;

  navigateURL: string;

  constructor(private _httpWrapper: HttpWrapper, private _router: Router, private activatedRoute: ActivatedRoute) {
  }

  ngOnInit() {
  }

  authenticationUser() {
    let that = this;

    this._httpWrapper.post({ url: "api/authentication/authenticateuser", data: that.authenticationModel }).then(function (response) {
      localStorage.setItem("userSession", JSON.stringify(response));
      that.tokenPayload = decode(JSON.stringify(response.token));
      that.navigateURL = 'home/'+ that.tokenPayload.primarysid;
      that._router.navigate([that.navigateURL]);

    }).catch(function (error) {
      alert(error);
    });
  }

  registerUser() {
    let that = this;
    that._httpWrapper.post({ url: "api/authentication/registeruser", data: that.registrationModel }).then(function (result) {


    }).catch(function (error) {
    });
  }

}
