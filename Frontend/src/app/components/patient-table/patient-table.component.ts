import { CdkTable } from '@angular/cdk/table';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTable } from '@angular/material/table';

import { Patient } from '../../models/patient';
import { PatientService } from '../../services/patient.service';

@Component({
  selector: 'patient-table',
  templateUrl: './patient-table.component.html',
  styleUrls: ['./patient-table.component.css']
})
export class PatientTableComponent implements OnInit {
  @ViewChild(MatTable) table!: MatTable<Patient>;

  columns: string[] = ['firstName', 'lastName', 'birthday', 'gender'] // order of columns displayed on table
  patientRecords: any = []      // data source for table

  constructor(private patientService: PatientService) {
  }
  
  ngOnInit(): void {
    this.getPatients();
  }

  getPatients(): void {
    // Get patients from patient service
    this.patientService.getPatients()
      .subscribe(patients => {
        this.patientRecords = patients;

        // refresh table
        this.table.renderRows();
      }, error => console.error(error));

    // Retry every 5in case of gateway timeout 504 (backend server still initiaiizing)
    // show loading message with circle animation 
  }
}
