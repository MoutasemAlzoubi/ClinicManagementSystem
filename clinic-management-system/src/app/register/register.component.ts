
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../Service/auth.service';
import { CountryService } from '../Service/CountryService';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  error = '';
  success = '';
  loading = false;
  countries: any[] = [];
  registerForm: FormGroup;

  constructor(formBuilder: FormBuilder, private auth: AuthService, private countryService: CountryService) {
    this.registerForm = formBuilder.group({
      firstName: ['', [Validators.required, Validators.maxLength(50)]],
      lastName: ['', [Validators.required, Validators.maxLength(50)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(3)]],
      dob: ['', Validators.required],
      gender: ['', Validators.required],
      phone: ['', Validators.required],
      address: ['', Validators.required],
      countryId: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.countryService.CountryList().subscribe({
      next: data => this.countries = data,
      error: () => this.error = 'Could not load countries.'
    });
  }

  register(): void {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.error = '';
    this.success = '';

    this.auth.registerPatient(this.registerForm.getRawValue()).subscribe({
      next: () => {
        this.success = 'Your account has been created. You can now sign in.';
        this.registerForm.reset();
      },
      error: error => {
        this.error = error.error?.message || 'Registration failed.';
        this.loading = false;
      },
      complete: () => this.loading = false
    });
  }
}
