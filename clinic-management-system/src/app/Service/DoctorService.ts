import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { DoctorDTO } from "../Model/DoctorDTO";
import { CreateDoctorDTO } from "../Model/CreateDoctorDTO";

@Injectable({ providedIn: 'root' })
export class DoctorService {
  private apiUrl = 'https://localhost:7253/api/Doctor';
  constructor(private httpClient: HttpClient) { }

  getTheDoctors(): Observable<DoctorDTO[]> { return this.httpClient.get<DoctorDTO[]>(this.apiUrl + '/all'); }
  getPublicDoctors(): Observable<DoctorDTO[]> { return this.httpClient.get<DoctorDTO[]>(this.apiUrl); }
  saveDoctor(doctor: CreateDoctorDTO): Observable<any> { return this.httpClient.post(this.apiUrl, doctor); }
  updateDoctor(doctor: DoctorDTO): Observable<any> { return this.httpClient.put(this.apiUrl, doctor); }
  deleteDoctor(id: number): Observable<any> { return this.httpClient.delete(this.apiUrl + '/' + id); }
  getMyProfile(): Observable<DoctorDTO> { return this.httpClient.get<DoctorDTO>(this.apiUrl + '/my-profile'); }
  getMyPatients(): Observable<any[]> { return this.httpClient.get<any[]>(this.apiUrl + '/my-patients'); }
}
