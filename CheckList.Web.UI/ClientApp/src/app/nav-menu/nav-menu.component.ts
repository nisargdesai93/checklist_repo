import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import decode from 'jwt-decode';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  navigateURL: string = '/login';
  userSessionModel: any;

  constructor(private _router: Router, private activatedRoute: ActivatedRoute) {
  }

  ngOnInit() {
    this.getLocalStorage();
  }


  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    let that = this;

    localStorage.clear();
    that._router.navigate([that.navigateURL]);
  }

  getLocalStorage() {
    let that = this;
    that.userSessionModel = JSON.parse(localStorage.getItem('userSession'));
  }
}
