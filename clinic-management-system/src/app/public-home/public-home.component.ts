import { Component, OnInit } from '@angular/core';
import { DoctorService } from '../Service/DoctorService';

@Component({ selector: 'app-public-home', templateUrl: './public-home.component.html', styleUrls: ['./public-home.component.css'] })
export class PublicHomeComponent implements OnInit {
  doctors: any[] = [];
  constructor(private doctorService: DoctorService) { }
  ngOnInit(): void {
    this.doctorService.getPublicDoctors().subscribe({
      next: doctors => this.doctors = doctors,
      error: () => this.doctors = []
    });
  }
}
