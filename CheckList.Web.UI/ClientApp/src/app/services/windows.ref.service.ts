import { Injectable } from '@angular/core';

function _window(): any {
  // return the global native browser window object
  return window;
}

@Injectable({ providedIn: 'root' })
export class WindowRef {
  static lookup: any;
  static lookupType: any;
  static lookupDb: any;
  static userIdentity: any;
  static userRoles: any;
  static selectedTZ: string;
  static softwareVersion: string;

  get nativeWindow(): any {
    return _window();
  }

  getLookup() {
    return WindowRef.lookup;
  }

  getLookupType() {
    return WindowRef.lookupType;
  }

  getLookupDb() {
    return WindowRef.lookupDb;
  }


  getUserIdentity() {
    return WindowRef.userIdentity;
  }

  getUserRoles() {
    return WindowRef.userRoles;
  }

  getSoftwareVersion() {
    return WindowRef.softwareVersion;
  }

}
