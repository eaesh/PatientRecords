import { CdkTable } from '@angular/cdk/table';
import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTable, MatTableDataSource } from '@angular/material/table';

import { Patient } from '../../models/patient';
import { PatientService } from '../../services/patient.service';

@Component({
  selector: 'patient-table',
  templateUrl: './patient-table.component.html',
  styleUrls: ['./patient-table.component.css']
})
export class PatientTableComponent implements OnInit {
  @ViewChild(MatTable) table!: MatTable<Patient>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  columns: string[] = ['firstName', 'lastName', 'birthday', 'gender'] // order of columns displayed on table
  patientRecords: MatTableDataSource<Patient> = new MatTableDataSource<Patient>();   // data source for table

  constructor(private patientService: PatientService) {
  }
  
  ngOnInit(): void {
    this.getPatients();
  }

  getPatients(): void {
    // Get patients from patient service
    this.patientService.getPatients()
      .subscribe(patients => {
        this.patientRecords = new MatTableDataSource(patients);
        this.patientRecords.paginator = this.paginator;
        this.patientRecords.sort = this.sort;
      }, error => console.error(error));

    // Retry every 5in case of gateway timeout 504 (backend server still initiaiizing)
    // show loading message with circle animation 
  }


  onFileUpload() {
    // update table after upload
    this.getPatients();
  }
}
