import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { CountryDTO } from "../Model/CountryDTO";

@Injectable({ providedIn: 'root' })
export class CountryService {
  private apiUrl = 'https://localhost:7253/api/Country';
  constructor(private httpClient: HttpClient) { }

  CountryList(): Observable<CountryDTO[]> {
    return this.httpClient.get<CountryDTO[]>(this.apiUrl + '/LoadAll');
  }

  Insert(country: CountryDTO): Observable<any> {
    return this.httpClient.post(this.apiUrl, country);
  }

  EditCountry(id: number): Observable<CountryDTO> {
    return this.httpClient.get<CountryDTO>(this.apiUrl + '/LoadCountry?countryId=' + id);
  }

  UpdateCountry(country: CountryDTO): Observable<any> {
    return this.httpClient.put(this.apiUrl, country);
  }

  DeleteCountry(id: number): Observable<any> {
    return this.httpClient.delete(this.apiUrl + '?Id=' + id);
  }
}
