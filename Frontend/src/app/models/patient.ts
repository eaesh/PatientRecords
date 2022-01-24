export interface Patient {
  id: number;
  firstName: string;
  lastName: string;
  birthday: string;
  gender: string;

  isEdit: boolean;    // for table view use only
}
