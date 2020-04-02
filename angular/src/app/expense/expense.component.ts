import { Component, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
    ExpenseServiceServiceProxy,
    ExpenseDto,
    ExpenseDtoPagedResultDto} from '@shared/service-proxies/service-proxies';
import { CreateExpenseDialogComponent } from './create-expense/create-expense-dialog.component';
import { EditExpenseDialogComponent } from './edit-expense/edit-expense-dialog.component';

class PagedExpenseRequestDto extends PagedRequestDto {
    keyword: string;
}

@Component({
    templateUrl: './expense.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
      ]
})
export class ExpenseComponent extends PagedListingComponentBase<ExpenseDto> {
    expenses: ExpenseDto[] = [];
    keyword = '';

    constructor(
        injector: Injector,
        private _expenseService: ExpenseServiceServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    list(
        request: PagedExpenseRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {

        request.keyword = this.keyword;

        this._expenseService
            .getPaginatedAll(request.keyword,undefined,undefined, request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: ExpenseDtoPagedResultDto) => {
                this.expenses = result.items;
                console.log("Expenses",this.expenses);
                this.showPaging(result, pageNumber);
            });
    }

    delete(expense: ExpenseDto): void {
        abp.message.confirm(
            this.l('ExpenseDeleteWarningMessage', expense.name),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._expenseService
                        .delete(expense.id)
                        .pipe(
                            finalize(() => {
                                abp.notify.success(this.l('SuccessfullyDeleted'));
                                this.refresh();
                            })
                        )
                        .subscribe(() => { });
                }
            }
        );
    }

    createExpense(): void {
        this.showCreateOrEditExpenseDialog();
    }

    editExpense(expense: ExpenseDto): void {
        this.showCreateOrEditExpenseDialog(expense.id);
    }

    showCreateOrEditExpenseDialog(id?: number): void {
        let createOrEditExpenseDialog;
        if (id === undefined || id <= 0) {
            createOrEditExpenseDialog = this._dialog.open(CreateExpenseDialogComponent);
        } else {
            createOrEditExpenseDialog = this._dialog.open(EditExpenseDialogComponent, {
                data: id
            });
        }

        createOrEditExpenseDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }

    clearFilters(){
        this.keyword = "";
    }
}
