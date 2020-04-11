import { Component, Injector,OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ExpenseServiceServiceProxy,
    CreateExpenseDto,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'create-expense-dialog.component.html',
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
export class CreateExpenseDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  expense: CreateExpenseDto = new CreateExpenseDto();

  constructor(
    injector: Injector,
    public _expenseService: ExpenseServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateExpenseDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
  }

  save(): void {
    this.saving = true;
    this.expense.tenantId = this.appSession.tenantId;

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
