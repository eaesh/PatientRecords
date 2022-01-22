import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'patient-table',
  templateUrl: './patient-table.component.html',
  styleUrls: ['./patient-table.component.css']
})
export class PatientTableComponent implements OnInit {
  // change birthday to string for date only
  columns: string[] = [ 'firstName', 'lastName', 'birthday', 'gender' ]
  patientRecords = [
    { firstName: 'Fudge', lastName: 'Cake', birthday: new Date('2020-01-01'), gender: 'M' },
    { firstName: 'Foo', lastName: 'Bar', birthday: new Date('1990-12-24'), gender: 'F' }
  ];

  constructor() {
  }

  ngOnInit(): void {

  }
}


export interface Patient {
  firstName: string;
  lastName: string;
  birthday: Date;
  gender: string;
}
