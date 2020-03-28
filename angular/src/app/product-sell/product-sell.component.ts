import { Component, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
    ProductSellServiceServiceProxy,
    ProductSellDto,
    ProductSellDtoPagedResultDto} from '@shared/service-proxies/service-proxies';
import { CreateProductSellDialogComponent } from './create-product-sell/create-product-sell-dialog.component';
import { EditProductSellDialogComponent } from './edit-product-sell/edit-product-sell-dialog.component';

class PagedProductSellRequestDto extends PagedRequestDto {
    keyword: string;
}

@Component({
    templateUrl: './product-sell.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
      ]
})
export class ProductSellComponent extends PagedListingComponentBase<ProductSellDto> {
    productSells: ProductSellDto[] = [];
    keyword = '';

    constructor(
        injector: Injector,
        private _productSellService: ProductSellServiceServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    list(
        request: PagedProductSellRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {
        request.keyword = this.keyword;

        this._productSellService
            .getPaginatedAll(request.keyword,undefined,undefined, undefined,request.skipCount,request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: any) => {
                this.productSells = result.items;
                this.showPaging(result, pageNumber);
            });
        // this._productSellService.getAll().subscribe(result=>{
        //     this.productSells = result;
        // } )
    }

    delete(productSell: ProductSellDto): void {
        abp.message.confirm(
            this.l('ProductSellDeleteWarningMessage', productSell.status),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._productSellService
                        .delete(productSell.id)
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

    createProductSell(): void {
        this.showCreateOrEditProductSellDialog();
    }

    editProductSell(productSell: ProductSellDto): void {
        this.showCreateOrEditProductSellDialog(productSell.id);
    }

    showCreateOrEditProductSellDialog(id?: number): void {
        let createOrEditProductSellDialog;
        if (id === undefined || id <= 0) {
            createOrEditProductSellDialog = this._dialog.open(CreateProductSellDialogComponent);
        } else {
            createOrEditProductSellDialog = this._dialog.open(EditProductSellDialogComponent, {
                data: id
            });
        }

        createOrEditProductSellDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }
}
