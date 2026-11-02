export class PatientDTO {
  id!: number;
  firstName!: string;
  lastName!: string;
  dob!: Date;
  email!: string;
  gender!: string;
  address!: string;
  phone!: string;
  countryId!: number;
  countryName?: string;
  userId?: string;
}
