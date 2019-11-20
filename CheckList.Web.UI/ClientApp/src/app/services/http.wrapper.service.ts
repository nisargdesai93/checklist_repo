import { Injectable } from "@angular/core"
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Router } from "@angular/router";
import { WindowRef } from "./windows.ref.service";
import { Deferred } from "./deferred";
import { NotificationMessages } from "./notification.service";
import { Response } from "@angular/http";
import { promise } from "selenium-webdriver";


enum HttpMethod {
  Get = 1, Post = 2, Put = 3, Delete = 4
}

interface HttpRequestOptions {
  url?: string;
  method?: string;
  search?: string;
  headers?: any;
  body?: any;
}

@Injectable({ providedIn: 'root' })
export class HttpWrapper {

  defaultValues: any
  apiEndPoint: string

  constructor(
    private _http: HttpClient,
    private _window: WindowRef,
    private _router: Router,
    private _notification: NotificationMessages

  ) {
    this.apiEndPoint = "https://localhost:5001/";
    let that = this;
    this.defaultValues = (function (httpMethod: any) {
      return {
        url: '',
        data: '',
        showOnSuccess: true,
        showOnFailure: true,
        disableNotification: false,
        method: httpMethod,
        success: function (deffered: any, result: any, def: any) {
          var msg = '';
          if (def.showOnSuccess && msg && msg.length > 0) {
            if (!def.disableNotification && def.method != HttpMethod.Get) {
              that._notification.success(msg);
            }
          }

          return deffered.resolve(result);
        },
        error: function (deffered: any, result: any, def: any) {
          var errorMessage = '';

          try {
            errorMessage = result.message || JSON.parse(result._body).message || 'Some Error Occurred';
          }
          catch (e) {
            errorMessage = 'Some Error Occurred';
          }
          finally {
            that._notification.error(errorMessage);
            return deffered.reject(errorMessage);
          }
        }
      };
    });
  }


  httpRequestOptions(defaultOptions: any): HttpRequestOptions {
    let output: HttpRequestOptions = {};

    if (defaultOptions.headers && defaultOptions.headers.skipfullpageloader) {
      var headers = new HttpHeaders();
      headers.append('skipfullpageloader', 'true');
      output.headers = headers;
    }

    return output;
  }

  get(opts: any): Promise<any> {
    let that = this;
    var deferred = new Deferred<any>();
    var def = Object.assign(this.defaultValues(HttpMethod.Get), opts);
    var options = this.httpRequestOptions(def);
    this._http.get(this.apiEndPoint + def.url, options)
      .subscribe(
        function (data: any) {
          return def.success(deferred, data, def);
        },
        function (error: any) {
          return def.error(deferred, error, def);
        }
      );
    return deferred.promise;
  }

  delete(opts: any): Promise<any> {
    let that = this;
    var deferred = new Deferred<any>();
    var def = Object.assign(this.defaultValues(HttpMethod.Delete), opts);
    var options = this.httpRequestOptions(def);
    this._http.delete(this.apiEndPoint + def.url, options)
      .subscribe(
        function (data: any) {

          return def.success(deferred, data, def)
        },
        function (error: any) {
          return def.error(deferred, error, def);
        }
      );

    return deferred.promise;
  }

  post(opts: any): Promise<any> {
    let that = this;
    var deferred = new Deferred<any>();
    var def = Object.assign(this.defaultValues(HttpMethod.Post), opts);
    var options = this.httpRequestOptions(def);
    this._http.post(this.apiEndPoint + def.url, def.data, options)
      .subscribe(
        function (data: any) {
          return def.success(deferred, data, def)
        },
        function (error: any) {
          return def.error(deferred, error, def);
        }
      );

    return deferred.promise;
  }

  put(opts: any): Promise<any> {
    let that = this;
    var deferred = new Deferred<any>();
    var def = Object.assign(this.defaultValues(HttpMethod.Put), opts);
    this._http.put(this.apiEndPoint + def.url, def.data)
      .subscribe(
        function (data) {
          return def.success(deferred, data, def)
        },
        function (error) {
          return def.error(deferred, error, def);
        }
      );

    return deferred.promise;
  }

  upload(opts: any): Promise<any> {
    let that = this;
    var deferred = new Deferred<any>();
    var def = Object.assign(this.defaultValues(HttpMethod.Post), opts);
    this._http.post(this.apiEndPoint + def.url, def.data)
      .subscribe(
        function (data) {
          return def.success(deferred, data, def)
        },
        function (error) {
          return def.error(deferred, error, def);
        }
      );

    return deferred.promise;
  }
}
