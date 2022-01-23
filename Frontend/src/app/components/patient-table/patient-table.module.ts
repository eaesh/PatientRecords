import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material/table';

import { PatientFileUpload } from '../patient-file-upload/patient-file-upload.component';
import { PatientTableComponent } from './patient-table.component';


@NgModule({
  declarations: [
    PatientFileUpload,
    PatientTableComponent,
  ],
  imports: [
    MatTableModule
  ],
  exports: [
    PatientTableComponent
  ]
})
export class PatientTableModule { }
