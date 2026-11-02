import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { PrescriptionDTO } from "../Model/PrescriptionDTO";

@Injectable({ providedIn: 'root' })
export class PrescriptionService {
  private apiUrl = 'https://localhost:7253/api/Prescription';
  constructor(private httpClient: HttpClient) { }
  getAll(): Observable<PrescriptionDTO[]> { return this.httpClient.get<any[]>(this.apiUrl); }
  getMyPrescriptions(): Observable<PrescriptionDTO[]> { return this.httpClient.get<any[]>(this.apiUrl + '/my-prescriptions'); }
  save(prescription: PrescriptionDTO): Observable<any> { return this.httpClient.post(this.apiUrl, prescription); }
}
