
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Service/auth.service';
import { DoctorService } from '../Service/DoctorService';
import { AppointmentService } from '../Service/AppointmentService';
import { MedicalRecordService } from '../Service/MedicalRecordService';
import { PrescriptionService } from '../Service/PrescriptionService';

@Component({
  selector: 'app-patient-dashboard',
  templateUrl: './patient-dashboard.component.html',
  styleUrls: ['./patient-dashboard.component.css']
})
export class PatientDashboardComponent implements OnInit {
  user: any;
  doctors: any[] = [];
  appointments: any[] = [];
  records: any[] = [];
  prescriptions: any[] = [];
  message = '';
  error = '';

  appointmentTouched = {
    doctorId: false,
    startDateTime: false,
    endDateTime: false
  };

  appointment = {
    doctorId: '',
    startDateTime: '',
    endDateTime: '',
    notes: ''
  };

  constructor(
    private auth: AuthService,
    private doctorService: DoctorService,
    private appointmentService: AppointmentService,
    private medicalRecordService: MedicalRecordService,
    private prescriptionService: PrescriptionService
  ) {
    this.user = auth.getUser();
  }

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.doctorService.getPublicDoctors().subscribe({
      next: data => this.doctors = data,
      error: () => this.doctors = []
    });

    this.appointmentService.getMyAppointments().subscribe({
      next: data => this.appointments = data,
      error: () => this.appointments = []
    });

    this.medicalRecordService.getMyRecords().subscribe({
      next: data => this.records = data,
      error: () => this.records = []
    });

    this.prescriptionService.getMyPrescriptions().subscribe({
      next: data => this.prescriptions = data,
      error: () => this.prescriptions = []
    });
  }

  isStartDateInvalid(): boolean {
    if (!this.appointment.startDateTime) return false;

    const startDate = new Date(this.appointment.startDateTime);

    return isNaN(startDate.getTime()) || startDate <= new Date();
  }

  isEndDateInvalid(): boolean {
    if (!this.appointment.endDateTime) return false;

    const endDate = new Date(this.appointment.endDateTime);

    if (isNaN(endDate.getTime())) return true;

    if (!this.appointment.startDateTime) return false;

    const startDate = new Date(this.appointment.startDateTime);

    return !isNaN(startDate.getTime()) && endDate <= startDate;
  }

  book(): void {
    this.appointmentTouched = {
      doctorId: true,
      startDateTime: true,
      endDateTime: true
    };

    this.error = '';
    this.message = '';

    if (
      !this.appointment.doctorId ||
      !this.appointment.startDateTime ||
      !this.appointment.endDateTime ||
      this.isStartDateInvalid() ||
      this.isEndDateInvalid()
    ) {
      return;
    }

    const startDate = new Date(this.appointment.startDateTime);
    const endDate = new Date(this.appointment.endDateTime);

    this.appointmentService.book({
      ...this.appointment,
      doctorId: Number(this.appointment.doctorId),
      startDateTime: startDate.toISOString(),
      endDateTime: endDate.toISOString()
    }).subscribe({
      next: () => {
        this.message = 'Appointment request submitted.';
        this.appointment = {
          doctorId: '',
          startDateTime: '',
          endDateTime: '',
          notes: ''
        };
        this.appointmentTouched = {
          doctorId: false,
          startDateTime: false,
          endDateTime: false
        };
        this.load();
      },
      error: (e: any) => this.error = e.error?.message || 'Could not book appointment.'
    });
  }

  cancel(id: number): void {
    this.error = '';
    this.message = '';

    this.appointmentService.cancel(id).subscribe({
      next: () => {
        this.message = 'Appointment cancelled.';
        this.load();
      },
      error: () => this.error = 'Could not cancel appointment.'
    });
  }

  logout(): void {
    this.auth.logout();
  }
}
