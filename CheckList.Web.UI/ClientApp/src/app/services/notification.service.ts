import { Injectable } from "@angular/core";

@Injectable({ providedIn: 'root' })
export class NotificationMessages {

  constructor() { }

  appendCustomOptions(appendTo: any, appendFrom: any) {
    if (appendFrom)
      Object.assign(appendTo, appendFrom);
  }

  success(msg: string, customOptions?: any) {
    let options = { cssClass: 'alert-success ' };
    this.appendCustomOptions(options, customOptions);
    this.showMessage(msg, options);
  }

  error(msg: string, customOptions?: any) {
    let options = { cssClass: 'alert-danger' };
    this.appendCustomOptions(options, customOptions);
    this.showMessage(msg, options);
  }

  warning(msg: string, customOptions?: any) {
    let options = { cssClass: 'alert-warning ' };
    this.appendCustomOptions(options, customOptions);
    this.showMessage(msg, options);
  }

  private showMessage(msg: string, options: any) {
    //this._notification.show(msg, options);
  }
}
