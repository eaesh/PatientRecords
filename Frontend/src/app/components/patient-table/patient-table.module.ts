import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';

import { PatientFileUpload } from '../patient-file-upload/patient-file-upload.component';
import { PatientTableComponent } from './patient-table.component';


@NgModule({
  declarations: [
    PatientFileUpload,
    PatientTableComponent,
  ],
  imports: [
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule
  ],
  exports: [
    PatientTableComponent
  ]
})
export class PatientTableModule { }
