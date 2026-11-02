import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PatientDTO } from "../Model/PatientDTO";

@Injectable({ providedIn: 'root' })
export class PatientService {
  private apiUrl = 'https://localhost:7253/api/Patient';
  constructor(private httpClient: HttpClient) { }

  savePatient(patient: PatientDTO): Observable<any> { return this.httpClient.post(this.apiUrl, patient); }
  getThePatients(): Observable<PatientDTO[]> { return this.httpClient.get<PatientDTO[]>(this.apiUrl + '/GetThePatients'); }
  deletePatient(id: number): Observable<any> { return this.httpClient.delete(this.apiUrl + '?id=' + id); }
  editPatient(id: number): Observable<PatientDTO> { return this.httpClient.get<PatientDTO>(this.apiUrl + '/GetThePatient?PatientId=' + id); }
  updatePatient(patient: PatientDTO): Observable<any> { return this.httpClient.put(this.apiUrl, patient); }
  getMyProfile(): Observable<PatientDTO> { return this.httpClient.get<PatientDTO>(this.apiUrl + '/my-profile'); }
}
