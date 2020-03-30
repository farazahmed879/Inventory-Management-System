import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ExpenseDto, ExpenseServiceServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-expense-dialog.component.html',
  styles: [
    `
      mat-form-field {
        width: 100%;
      }
      mat-checkbox {
        padding-bottom: 5px;
      }
    `
  ]
})
export class EditExpenseDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  expense: ExpenseDto = new ExpenseDto();

  constructor(
    injector: Injector,
    public _expenseService: ExpenseServiceServiceProxy,
    private _dialogRef: MatDialogRef<EditExpenseDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._expenseService.getById(this._id).subscribe((result: ExpenseDto) => {
      this.expense = result;
    });
  }

  save(): void {
    this.saving = true;

    this._expenseService
      .createOrEdit(this.expense)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.close(true);
      });
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
