import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AppointmentDTO } from "../Model/AppointmentDTO";
import { CreateAppointmentDTO } from "../Model/CreateAppointmentDTO";
import { UpdateAppointmentStatusDTO } from "../Model/UpdateAppointmentStatusDTO";

@Injectable({ providedIn: 'root' })
export class AppointmentService {
  private apiUrl = 'https://localhost:7253/api/Appointment';
  constructor(private httpClient: HttpClient) { }
  getAll(): Observable<AppointmentDTO[]> { return this.httpClient.get<any[]>(this.apiUrl); }
  getMyAppointments(): Observable<AppointmentDTO[]> { return this.httpClient.get<any[]>(this.apiUrl + '/my-appointments'); }
  getDoctorAppointments(): Observable<AppointmentDTO[]> { return this.httpClient.get<any[]>(this.apiUrl + '/doctor-appointments'); }
  book(appointment: CreateAppointmentDTO): Observable<any> { return this.httpClient.post(this.apiUrl, appointment); }
  updateStatus(id: number, status: string): Observable<any> { return this.httpClient.put(this.apiUrl + '/' + id + '/status', { status: status }); }
  cancel(id: number): Observable<any> { return this.httpClient.put(this.apiUrl + '/' + id + '/cancel', {}); }
}
