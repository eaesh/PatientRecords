import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';

import { Patient } from '../models/patient';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private patientsUrl = '/api/patients'
  mockPatients: Patient[] = [
    { firstName: 'Fudge', lastName: 'Cake', birthday: '2020-01-01', gender: 'M' },
    { firstName: 'Foo', lastName: 'Bar', birthday: '1990-12-24', gender: 'F' }
  ];

  constructor(private http: HttpClient) { }

  getPatients(): Observable<Patient[]> {
    //return of(this.mockPatients);
    return this.http.get<Patient[]>(this.patientsUrl);
  }

}
