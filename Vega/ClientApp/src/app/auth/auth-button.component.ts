import { Component } from '@angular/core';
import { async } from '@angular/core/testing';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-auth-button',
  template: `
    <button
      class="btn btn-link nav-link text-dark"
      *ngIf="!isAuthenticated() && !(this.auth.isAuthenticated$ | async)"
      (click)="auth.loginWithRedirect()"
    >
      Log in
    </button>
    <button
      class="btn btn-link nav-link text-dark"
      *ngIf="isAuthenticated() || (this.auth.isAuthenticated$ | async)"
      (click)="logout()"
    >
      Log out
    </button>
  `
})
export class AuthButtonComponent {
  constructor(public auth: AuthService) {}

  protected readonly window = window;

  logout() {
    localStorage.removeItem('token');
    this.auth.logout({ returnTo: window.location.origin });
  }

  isAuthenticated() {
    return localStorage.getItem('token');
  }
}
