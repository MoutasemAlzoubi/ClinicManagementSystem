import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../Service/auth.service';

@Component({ selector: 'app-login', templateUrl: './login.component.html', styleUrls: ['./login.component.css'] })
export class LoginComponent {
  error = '';
  loading = false;
  loginForm: FormGroup;
  constructor(formBuilder: FormBuilder, private auth: AuthService, private router: Router) {
    this.loginForm = formBuilder.group({ email: ['', [Validators.required, Validators.email]], password: ['', Validators.required] });
  }
  login(): void {
    if (this.loginForm.invalid) return;
    this.loading = true; this.error = '';
    this.auth.login(this.loginForm.getRawValue()).subscribe({
      next: () => this.router.navigate(['/portal']),
      error: error => { this.error = error.error?.message || 'Unable to sign in. Check your credentials.'; this.loading = false; },
      complete: () => this.loading = false
    });
  }
}
