import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { FlatpickrModule } from 'angularx-flatpickr';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './Shared/login.component';
import { LargeCalenderComponent } from './calender/large.calender.component';
import { NgbModule, NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';

import { AuthGuardService as AuthGuard, AuthGuardService } from './services/auth-gaurd-service';
import { JwtAuthService } from './services/JwtAuthService';
import { JwtModule, JwtModuleOptions } from '@auth0/angular-jwt';
import { HttpModule } from '@angular/http';

const JWT_Module_Options: JwtModuleOptions = {
  config: {
    tokenGetter: function tokenGetter() {
      return localStorage.getItem('userSession');
    }
  }
};

const routes: Routes = [
  { path: 'home/:userId', component: HomeComponent, pathMatch: 'full', canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent, pathMatch: 'full' },
];


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    LargeCalenderComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule, HttpModule, FormsModule,
    JwtModule.forRoot(JWT_Module_Options),
    RouterModule.forRoot(routes),
    CommonModule,
    BrowserAnimationsModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    }),
    NgbModule,
    NgbModalModule,
    FlatpickrModule.forRoot()
  ],
  providers: [AuthGuardService, JwtAuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
