import { Component, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
    ProductSaleServiceServiceProxy,
    ProductSaleDto,
    ProductSaleDtoPagedResultDto} from '@shared/service-proxies/service-proxies';
import { CreateProductSaleDialogComponent } from './create-product-sale/create-product-sale-dialog.component';
import { EditProductSaleDialogComponent } from './edit-product-sale/edit-product-sale-dialog.component';

class PagedProductSaleRequestDto extends PagedRequestDto {
    keyword: string;
}

@Component({
    templateUrl: './product-sale.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
      ]
})
export class ProductSaleComponent extends PagedListingComponentBase<ProductSaleDto> {
    productSales: ProductSaleDto[] = [];
    keyword = '';

    constructor(
        injector: Injector,
        private _productSaleService: ProductSaleServiceServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    list(
        request: PagedProductSaleRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {
        request.keyword = this.keyword;

        this._productSaleService
            .getPaginatedAll(request.keyword,undefined,undefined, undefined,request.skipCount,request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: any) => {
                this.productSales = result.items;
                this.showPaging(result, pageNumber);
            });
        // this._productSaleService.getAll().subscribe(result=>{
        //     this.productSales = result;
        // } )
    }

    delete(productSale: ProductSaleDto): void {
        abp.message.confirm(
            this.l('ProductSaleDeleteWarningMessage', productSale.status),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._productSaleService
                        .delete(productSale.id)
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

    createProductSale(): void {
        this.showCreateOrEditProductSaleDialog();
    }

    editProductSale(productSale: ProductSaleDto): void {
        this.showCreateOrEditProductSaleDialog(productSale.id);
    }

    showCreateOrEditProductSaleDialog(id?: number): void {
        let createOrEditProductSaleDialog;
        if (id === undefined || id <= 0) {
            createOrEditProductSaleDialog = this._dialog.open(CreateProductSaleDialogComponent);
        } else {
            createOrEditProductSaleDialog = this._dialog.open(EditProductSaleDialogComponent, {
                data: id
            });
        }

        createOrEditProductSaleDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }

    clearFilters() {
        this.keyword = "";
      }
}
