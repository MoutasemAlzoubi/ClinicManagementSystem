export class MedicalRecordsDTO {
  id?: number;
  recordDate?: Date;
  observation!: string;
  diagnosis!: string;
  treatment!: string;
  notes!: string;
  doctorId?: number;
  patientId!: number;
}
