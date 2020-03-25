import { Component, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
    CompanyServiceServiceProxy,
    CompanyDto,
    CompanyDtoPagedResultDto} from '@shared/service-proxies/service-proxies';
import { CreateCompanyDialogComponent } from './create-company/create-company-dialog.component';
import { EditCompanyDialogComponent } from './edit-company/edit-company-dialog.component';

class PagedCompanyRequestDto extends PagedRequestDto {
    keyword: string;
}

@Component({
    templateUrl: './company.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
      ]
})
export class CompanyComponent extends PagedListingComponentBase<CompanyDto> {
    companys: CompanyDto[] = [];
    keyword = '';

    constructor(
        injector: Injector,
        private _companyService: CompanyServiceServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    list(
        request: PagedCompanyRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {

        request.keyword = this.keyword;

        this._companyService
            .getPaginatedAll(request.keyword, request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: CompanyDtoPagedResultDto) => {
                this.companys = result.items;
                this.showPaging(result, pageNumber);
            });
    }

    delete(company: CompanyDto): void {
        abp.message.confirm(
            this.l('CompanyDeleteWarningMessage', company.name),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._companyService
                        .delete(company.id)
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

    createCompany(): void {
        this.showCreateOrEditCompanyDialog();
    }

    editCompany(company: CompanyDto): void {
        this.showCreateOrEditCompanyDialog(company.id);
    }

    showCreateOrEditCompanyDialog(id?: number): void {
        let createOrEditCompanyDialog;
        if (id === undefined || id <= 0) {
            createOrEditCompanyDialog = this._dialog.open(CreateCompanyDialogComponent);
        } else {
            createOrEditCompanyDialog = this._dialog.open(EditCompanyDialogComponent, {
                data: id
            });
        }

        createOrEditCompanyDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }
}
