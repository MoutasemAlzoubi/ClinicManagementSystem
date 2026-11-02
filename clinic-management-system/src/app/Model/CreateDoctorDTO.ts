export class CreateDoctorDTO {
  firstName!: string;
  lastName!: string;
  email!: string;
  temporaryPassword!: string;
  specialization!: string;
  bio?: string;
}
