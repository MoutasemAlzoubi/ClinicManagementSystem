export class CreateAppointmentDTO {
  doctorId!: number;
  startDateTime!: string;
  endDateTime!: string;
  notes?: string;
}
