import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Customer } from '../customer.model';
import { FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'crm-customer-create-dialog',
  templateUrl: './customer-create-dialog.component.html',
  styleUrls: ['./customer-create-dialog.component.scss']
})
export class CustomerCreateDialogComponent implements OnInit {
  detailForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<CustomerCreateDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Customer | null
  ) {
    this.detailForm = this.fb.group({
      firstName: [''],
      lastName: [''],
      phoneNumber: [''],
      emailAddress: [''],
      preferredContactMethod: ['email']
    });
    if (this.data) {
      this.detailForm.patchValue(this.data);
    }
   }

  ngOnInit(): void {
  }

  cancel(): void {
    // TODO: close this dialog
    this.dialogRef.close();
  }

  save(): void {
    // TODO: get form data and return to parent component
    const customer = {};
    this.dialogRef.close(customer);
  }

}
