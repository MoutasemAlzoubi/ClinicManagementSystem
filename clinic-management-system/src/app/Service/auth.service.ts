import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

export interface LoginResponse { token: string; expiresAt: string; email: string; fullName: string; roles: string[]; }
@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'https://localhost:7253/api/Account';
  constructor(private http: HttpClient, private router: Router) {}
  login(data: { email: string; password: string }): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, data).pipe(tap(result => {
      localStorage.setItem('clinic_token', result.token);
      localStorage.setItem('clinic_user', JSON.stringify({ email: result.email, fullName: result.fullName, roles: result.roles }));
    }));
  }
  registerPatient(data: any): Observable<any> { return this.http.post(`${this.apiUrl}/register-patient`, data); }
  getToken(): string | null { return localStorage.getItem('clinic_token'); }
  getUser(): any { try { return JSON.parse(localStorage.getItem('clinic_user') || 'null'); } catch { return null; } }
  getRole(): string | null { return this.getUser()?.roles?.[0] || null; }
  isLoggedIn(): boolean { return !!this.getToken(); }
  logout(): void { localStorage.removeItem('clinic_token'); localStorage.removeItem('clinic_user'); this.router.navigate(['/login']); }
}
