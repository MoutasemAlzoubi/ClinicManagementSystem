import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../Service/auth.service';
@Component({ selector: 'app-portal', template: '' })
export class PortalComponent implements OnInit {
  constructor(private auth: AuthService, private router: Router) { }
  ngOnInit(): void {
    const role = this.auth.getRole();
    if (role === 'Admin') this.router.navigate(['/admin']);
    else if (role === 'Doctor') this.router.navigate(['/doctor']);
    else if (role === 'Patient') this.router.navigate(['/patient']);
    else this.router.navigate(['/login']);
  }
}
