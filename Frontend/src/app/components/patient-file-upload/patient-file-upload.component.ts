import { Input, OnInit } from "@angular/core";
import { Component } from "@angular/core";
import { PatientService } from "../../services/patient.service";



@Component({
  selector: 'patient-file-upload',
  templateUrl: './patient-file-upload.component.html',
  styleUrls: []
})
export class PatientFileUpload implements OnInit {
  @Input() testFunction!: () => void;

  reader: FileReader = new FileReader();

  constructor(private patientService: PatientService) { }

  ngOnInit() {
    // define post-read action
    this.reader.onload = (e) => {
      this.uploadFile(this.reader.result?.toString());
    }

    this.testFunction();
  }

  //handle file upload event
  onFileInput(event: any) {
    // limit file type
    // limit number of files
    const file: File = event.target.files.item(0);

    // read file
    this.reader.readAsText(file);

  }

  // post file content to backend
  uploadFile(records: string | undefined) {
    if (records) {
      this.patientService.uploadPatientsFile(records.substring(records.indexOf('\n') + 1))
        .subscribe(res => { }, error => console.error(error));
    }
    else {
      console.error("Invalid File Content")
    }
  }
}
