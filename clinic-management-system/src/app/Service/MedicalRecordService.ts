import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { MedicalRecordsDTO } from "../Model/MedicalRecordsDTO";

@Injectable({ providedIn: 'root' })
export class MedicalRecordService {
  private apiUrl = 'https://localhost:7253/api/MedicalRecord';
  constructor(private httpClient: HttpClient) { }
  getAll(): Observable<MedicalRecordsDTO[]> { return this.httpClient.get<any[]>(this.apiUrl); }
  getMyRecords(): Observable<MedicalRecordsDTO[]> { return this.httpClient.get<any[]>(this.apiUrl + '/my-records'); }
  getDoctorRecords(): Observable<MedicalRecordsDTO[]> { return this.httpClient.get<any[]>(this.apiUrl + '/doctor-records'); }
  save(record: MedicalRecordsDTO): Observable<any> { return this.httpClient.post(this.apiUrl, record); }
  update(record: MedicalRecordsDTO): Observable<any> { return this.httpClient.put(this.apiUrl, record); }
  delete(id: number): Observable<any> { return this.httpClient.delete(this.apiUrl + '/' + id); }
}
