import { Component, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
    TypeServiceServiceProxy,
    TypeDto,
    TypeDtoPagedResultDto} from '@shared/service-proxies/service-proxies';
import { CreateTypeDialogComponent } from './create-type/create-type-dialog.component';
import { EditTypeDialogComponent } from './edit-type/edit-type-dialog.component';

class PagedTypeRequestDto extends PagedRequestDto {
    keyword: string;
}

@Component({
    templateUrl: './type.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
      ]
})
export class TypeComponent extends PagedListingComponentBase<TypeDto> {
    types: TypeDto[] = [];
    keyword = '';

    constructor(
        injector: Injector,
        private _typeService: TypeServiceServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    list(
        request: PagedTypeRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {

        request.keyword = this.keyword;

        this._typeService
            .getPaginatedAll(request.keyword,this.appSession.tenantId,request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: TypeDtoPagedResultDto) => {
                this.types = result.items;
                this.showPaging(result, pageNumber);
            });
    }

    delete(type: TypeDto): void {
        abp.message.confirm(
            this.l('TypeDeleteWarningMessage', type.name),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._typeService
                        .delete(type.id)
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

    createType(): void {
        this.showCreateOrEditTypeDialog();
    }

    editType(type: TypeDto): void {
        this.showCreateOrEditTypeDialog(type.id);
    }

    showCreateOrEditTypeDialog(id?: number): void {
        let createOrEditTypeDialog;
        if (id === undefined || id <= 0) {
            createOrEditTypeDialog = this._dialog.open(CreateTypeDialogComponent);
        } else {
            createOrEditTypeDialog = this._dialog.open(EditTypeDialogComponent, {
                data: id
            });
        }

        createOrEditTypeDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }
}
