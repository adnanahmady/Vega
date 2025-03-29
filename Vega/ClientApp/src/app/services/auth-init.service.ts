import { Injectable } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Injectable({
  providedIn: 'root'
})
export class AuthInitService {
  constructor(private auth: AuthService) {
    this.initializeAuth();
  }

  private initializeAuth() {
    this.auth.isAuthenticated$.subscribe(isAuthenticated => {
      if (!isAuthenticated) return;

      this.auth.getAccessTokenSilently().subscribe(
        (token: string) => {
          localStorage.setItem('token', token);
          location.reload();
        },
      );
    });
  }
}

export function InitializeAuth(authInitService: AuthInitService) {
  return () => new Promise(resolve => {
    resolve(true);
  });
}