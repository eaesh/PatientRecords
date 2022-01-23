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

  columns: string[] = ['id', 'firstName', 'lastName', 'birthday', 'gender'] // order of columns displayed on table
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
        // Update data table
        this.patientRecords.data = patients;

        // Paginator
        this.patientRecords.paginator = this.paginator;

        // Sorting
        this.patientRecords.sort = this.sort;

        // Set filter by first or last name
        this.patientRecords.filterPredicate =
          (pat: Patient, filter: string) => pat.firstName.toLowerCase().startsWith(filter) || pat.lastName.toLowerCase().startsWith(filter);

      }, error => console.error(error));

    // ADD: Retry every 5 secs after gateway timeout error 504 (backend server still initiaiizing)
  }

  onFileUpload() {
    // update table after upload
    this.getPatients();
  }

  applyFilter(event: Event) {
    const filterValue: string = (event.target as HTMLInputElement).value;
    this.patientRecords.filter = filterValue.trim().toLowerCase();
  }
}
