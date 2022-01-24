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

  patientRecords: MatTableDataSource<Patient> = new MatTableDataSource<Patient>();   // data source for table
  columns: string[] = ['id', 'firstName', 'lastName', 'birthday', 'gender', 'isEdit'] // order of columns displayed on table
  genders: string[] = ['Male', 'Female']
  tableSchema: { [key: string]: string; } = {
    'firstName': 'text',
    'lastName': 'text',
    'birthday': 'text',
    'gender': 'text',
    'isEdit': 'isEdit'
  }

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

    // ADD: Retry every 5 secs after gateway timeout error 504 (backend server still initiailizing)
  }

  onFileUpload() {
    // update table after upload
    this.getPatients();
  }

  applyFilter(event: Event) {
    const filterValue: string = (event.target as HTMLInputElement).value;
    this.patientRecords.filter = filterValue.trim().toLowerCase();
  }

  editRow(patient: Patient) {

    if (patient.isEdit) {   // 'Done' is clicked

      // Update request for patient
      this.patientService.updatePatient(patient)
        .subscribe(() => {
        }, error => {
          console.error(error);
          // refresh table to revert
          this.getPatients();
        });
    }

    patient.isEdit = !patient.isEdit;
  }
}
