import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EMPTY, Observable, of } from 'rxjs';
import { catchError, retry } from 'rxjs/operators';

import { Patient } from '../models/patient';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private patientsUrl = '/api/patients'

  constructor(private http: HttpClient) { }

  getPatients(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.patientsUrl);
  }

  uploadPatientsFile(file: string): Observable<unknown> {
    console.log(file)

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.post(this.patientsUrl + "/upload", JSON.stringify(file), httpOptions);
  }
}
