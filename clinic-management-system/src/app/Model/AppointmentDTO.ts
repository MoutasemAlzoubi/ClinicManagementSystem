export class AppointmentDTO {
  id!: number;
  patientId!: number;
  patientName!: string;
  doctorId!: number;
  doctorName!: string;
  startDateTime!: Date;
  endDateTime!: Date;
  status!: string;
  notes!: string;
}
