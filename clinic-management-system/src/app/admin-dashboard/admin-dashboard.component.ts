
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Service/auth.service';
import { CountryService } from '../Service/CountryService';
import { PatientService } from '../Service/PatientService';
import { DoctorService } from '../Service/DoctorService';
import { AppointmentService } from '../Service/AppointmentService';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  user: any;
  countries: any[] = [];
  patients: any[] = [];
  doctors: any[] = [];
  appointments: any[] = [];
  message = '';
  error = '';

  patientTouched = {
    firstName: false, lastName: false, dob: false, email: false,
    gender: false, phone: false, address: false, countryId: false
  };

  doctorTouched = {
    firstName: false, lastName: false, email: false,
    temporaryPassword: false, specialization: false
  };

  countryTouched = false;

  patientForm = {
    firstName: '', lastName: '', dob: '', email: '',
    gender: '', address: '', phone: '', countryId: ''
  };

  doctorForm = {
    firstName: '', lastName: '', email: '',
    temporaryPassword: '', specialization: '', bio: ''
  };

  countryName = '';

  constructor(
    private auth: AuthService,
    private countryService: CountryService,
    private patientService: PatientService,
    private doctorService: DoctorService,
    private appointmentService: AppointmentService
  ) {
    this.user = auth.getUser();
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.countryService.CountryList().subscribe({
      next: data => this.countries = data,
      error: () => this.error = 'Could not load countries.'
    });

    this.patientService.getThePatients().subscribe({
      next: data => this.patients = data,
      error: () => this.error = 'Could not load patients.'
    });

    this.doctorService.getTheDoctors().subscribe({
      next: data => this.doctors = data,
      error: () => this.error = 'Could not load doctors.'
    });

    this.appointmentService.getAll().subscribe({
      next: data => this.appointments = data,
      error: () => this.error = 'Could not load appointments.'
    });
  }

  isValidEmail(email: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  }

  createPatient(): void {
    this.patientTouched = {
      firstName: true, lastName: true, dob: true, email: true,
      gender: true, phone: true, address: true, countryId: true
    };

    this.error = '';
    this.message = '';

    if (
      !this.patientForm.firstName.trim() ||
      !this.patientForm.lastName.trim() ||
      !this.patientForm.dob ||
      !this.patientForm.email.trim() ||
      !this.isValidEmail(this.patientForm.email) ||
      !this.patientForm.gender ||
      !this.patientForm.phone.trim() ||
      !this.patientForm.address.trim() ||
      !this.patientForm.countryId
    ) {
      return;
    }

    this.patientService.savePatient({
      ...this.patientForm,
      countryId: Number(this.patientForm.countryId)
    } as any).subscribe({
      next: () => {
        this.message = 'Patient record created.';
        this.patientForm = {
          firstName: '', lastName: '', dob: '', email: '',
          gender: '', address: '', phone: '', countryId: ''
        };
        this.patientTouched = {
          firstName: false, lastName: false, dob: false, email: false,
          gender: false, phone: false, address: false, countryId: false
        };
        this.load();
      },
      error: (e: any) => this.error = e.error?.message || 'Could not create patient.'
    });
  }

  createDoctor(): void {
    this.doctorTouched = {
      firstName: true, lastName: true, email: true,
      temporaryPassword: true, specialization: true
    };

    this.error = '';
    this.message = '';

    if (
      !this.doctorForm.firstName.trim() ||
      !this.doctorForm.lastName.trim() ||
      !this.doctorForm.email.trim() ||
      !this.isValidEmail(this.doctorForm.email) ||
      !this.doctorForm.temporaryPassword ||
      this.doctorForm.temporaryPassword.length < 8 ||
      !this.doctorForm.specialization.trim()
    ) {
      return;
    }

    this.doctorService.saveDoctor(this.doctorForm).subscribe({
      next: () => {
        this.message = 'Doctor account created.';
        this.doctorForm = {
          firstName: '', lastName: '', email: '',
          temporaryPassword: '', specialization: '', bio: ''
        };
        this.doctorTouched = {
          firstName: false, lastName: false, email: false,
          temporaryPassword: false, specialization: false
        };
        this.load();
      },
      error: (e: any) => {
        this.error = e.error?.message || e.error?.title || 'Could not create doctor.';
      }
    });
  }

  addCountry(): void {
    this.countryTouched = true;
    this.error = '';
    this.message = '';

    if (!this.countryName.trim()) return;

    this.countryService.Insert({
      id: 0,
      name: this.countryName
    } as any).subscribe({
      next: () => {
        this.message = 'Country added.';
        this.countryName = '';
        this.countryTouched = false;
        this.load();
      },
      error: (e: any) => this.error = e.error?.message || 'Could not add country.'
    });
  }

  status(id: number, status: string): void {
    this.appointmentService.updateStatus(id, status).subscribe({
      next: () => {
        this.message = 'Appointment updated.';
        this.load();
      },
      error: (e: any) => this.error = e.error?.message || 'Could not update appointment.'
    });
  }

  deleteCountry(id: number): void {
    this.countryService.DeleteCountry(id).subscribe({
      next: () => {
        this.message = 'Country deleted.';
        this.load();
      },
      error: (e: any) => this.error = e.error?.message || 'Country could not be deleted.'
    });
  }

  deletePatient(id: number): void {
    this.patientService.deletePatient(id).subscribe({
      next: () => {
        this.message = 'Patient deleted.';
        this.load();
      },
      error: (e: any) => this.error = e.error?.message || 'Patient could not be deleted.'
    });
  }

  deleteDoctor(id: number): void {
    this.doctorService.deleteDoctor(id).subscribe({
      next: () => {
        this.message = 'Doctor deleted.';
        this.load();
      },
      error: (e: any) => this.error = e.error?.message || 'Doctor could not be deleted. Deactivate doctors with clinical history.'
    });
  }

  toggleDoctor(doctor: any): void {
    this.doctorService.updateDoctor({
      ...doctor,
      isActive: !doctor.isActive
    }).subscribe({
      next: () => {
        this.message = 'Doctor status updated.';
        this.load();
      },
      error: (e: any) => this.error = e.error?.message || 'Could not update doctor.'
    });
  }

  logout(): void {
    this.auth.logout();
  }
}
