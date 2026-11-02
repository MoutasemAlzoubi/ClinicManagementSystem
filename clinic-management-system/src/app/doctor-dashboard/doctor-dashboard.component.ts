
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Service/auth.service';
import { DoctorService } from '../Service/DoctorService';
import { AppointmentService } from '../Service/AppointmentService';
import { MedicalRecordService } from '../Service/MedicalRecordService';
import { PrescriptionService } from '../Service/PrescriptionService';

@Component({
  selector: 'app-doctor-dashboard',
  templateUrl: './doctor-dashboard.component.html',
  styleUrls: ['./doctor-dashboard.component.css']
})
export class DoctorDashboardComponent implements OnInit {
  user: any;
  appointments: any[] = [];
  records: any[] = [];
  patients: any[] = [];
  message = '';
  error = '';

  recordTouched = {
    patientId: false, diagnosis: false, observation: false, treatment: false
  };

  prescriptionTouched = {
    patientId: false, medicationName: false, dosage: false,
    frequency: false, duration: false
  };

  record = {
    patientId: '', observation: '', diagnosis: '', treatment: '', notes: ''
  };

  prescription = {
    patientId: '', medicationName: '', dosage: '',
    frequency: '', duration: '', instructions: ''
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
    this.appointmentService.getDoctorAppointments().subscribe({
      next: data => this.appointments = data,
      error: () => this.error = 'Could not load appointments.'
    });

    this.medicalRecordService.getDoctorRecords().subscribe({
      next: data => this.records = data,
      error: () => this.records = []
    });

    this.doctorService.getMyPatients().subscribe({
      next: data => this.patients = data,
      error: () => this.patients = []
    });
  }

  createRecord(): void {
    this.recordTouched = {
      patientId: true, diagnosis: true, observation: true, treatment: true
    };

    this.error = '';
    this.message = '';

    if (
      !this.record.patientId ||
      !this.record.diagnosis.trim() ||
      !this.record.observation.trim() ||
      !this.record.treatment.trim()
    ) {
      return;
    }

    this.medicalRecordService.save({
      ...this.record,
      patientId: Number(this.record.patientId)
    }).subscribe({
      next: () => {
        this.message = 'Medical record saved.';
        this.record = {
          patientId: '', observation: '', diagnosis: '', treatment: '', notes: ''
        };
        this.recordTouched = {
          patientId: false, diagnosis: false, observation: false, treatment: false
        };
        this.load();
      },
      error: (e: any) => this.error = e.error?.message || 'Could not save medical record.'
    });
  }

  createPrescription(): void {
    this.prescriptionTouched = {
      patientId: true, medicationName: true, dosage: true,
      frequency: true, duration: true
    };

    this.error = '';
    this.message = '';

    if (
      !this.prescription.patientId ||
      !this.prescription.medicationName.trim() ||
      !this.prescription.dosage.trim() ||
      !this.prescription.frequency.trim() ||
      !this.prescription.duration.trim()
    ) {
      return;
    }

    this.prescriptionService.save({
      ...this.prescription,
      patientId: Number(this.prescription.patientId)
    }).subscribe({
      next: () => {
        this.message = 'Prescription created.';
        this.prescription = {
          patientId: '', medicationName: '', dosage: '',
          frequency: '', duration: '', instructions: ''
        };
        this.prescriptionTouched = {
          patientId: false, medicationName: false, dosage: false,
          frequency: false, duration: false
        };
      },
      error: (e: any) => this.error = e.error?.message || 'Could not create prescription.'
    });
  }

  logout(): void {
    this.auth.logout();
  }
}
