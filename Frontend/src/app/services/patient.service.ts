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

  updatePatient(patient: Patient) {
    const record: string =
      [patient.firstName, patient.lastName, patient.birthday, patient.gender.substring(0, 1)].join(',');

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.put(this.patientsUrl + "/" + patient.id, JSON.stringify(record), httpOptions)
  }

  uploadPatientsFile(file: string): Observable<unknown> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };

    return this.http.post(this.patientsUrl + "/upload", JSON.stringify(file), httpOptions);
  }
}
