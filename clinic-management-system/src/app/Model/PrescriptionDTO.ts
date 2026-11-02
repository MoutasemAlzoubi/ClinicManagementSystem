export class PrescriptionDTO {
  id?: number;
  patientId!: number;
  doctorId?: number;
  medicalRecordId?: number;
  medicationName!: string;
  dosage!: string;
  frequency!: string;
  duration!: string;
  instructions!: string;
  issuedAt?: Date;
}
