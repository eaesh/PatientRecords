import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material/table';

import { PatientTableComponent } from './patient-table.component';

@NgModule({
  declarations: [
    PatientTableComponent
  ],
  imports: [
    MatTableModule
  ],
  exports: [
    PatientTableComponent
  ]
})
export class PatientTableModule { }
