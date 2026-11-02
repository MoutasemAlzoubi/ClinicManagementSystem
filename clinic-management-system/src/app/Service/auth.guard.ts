import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { AuthService } from './auth.service';
@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}
  canActivate(route: ActivatedRouteSnapshot): boolean {
    if (!this.auth.isLoggedIn()) { this.router.navigate(['/login']); return false; }
    const allowed = route.data['roles'] as string[] | undefined;
    if (allowed && !allowed.some(role => this.auth.getUser()?.roles?.includes(role))) { this.router.navigate(['/portal']); return false; }
    return true;
  }
}
