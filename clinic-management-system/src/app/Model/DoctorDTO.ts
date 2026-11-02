export class DoctorDTO {
  id!: number;
  firstName!: string;
  lastName!: string;
  email?: string;
  specialization!: string;
  bio?: string;
  isActive?: boolean;
}
