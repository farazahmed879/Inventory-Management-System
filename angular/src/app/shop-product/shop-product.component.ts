import { Component, Injector } from '@angular/core';
import { MatDialog } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
    PagedListingComponentBase,
    PagedRequestDto
} from 'shared/paged-listing-component-base';
import {
    ShopProductServiceServiceProxy,
    ShopProductDto,
    ShopProductDtoPagedResultDto} from '@shared/service-proxies/service-proxies';
import { CreateShopProductDialogComponent } from './create-shop-product/create-shop-product-dialog.component';
import { EditShopProductDialogComponent } from './edit-shop-product/edit-shop-product-dialog.component';

class PagedShopProductRequestDto extends PagedRequestDto {
    keyword: string;
}

@Component({
    templateUrl: './shop-product.component.html',
    animations: [appModuleAnimation()],
    styles: [
        `
          mat-form-field {
            padding: 10px;
          }
        `
      ]
})
export class ShopProductComponent extends PagedListingComponentBase<ShopProductDto> {
    shopProducts: ShopProductDto[] = [];
    keyword = '';

    constructor(
        injector: Injector,
        private _shopProductService: ShopProductServiceServiceProxy,
        private _dialog: MatDialog
    ) {
        super(injector);
    }

    list(
        request: PagedShopProductRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ): void {

        request.keyword = this.keyword;

        this._shopProductService
            .getPaginatedAll(request.keyword,
                 undefined,
                 undefined,
                 undefined,
                 undefined,
                 request.skipCount, request.maxResultCount)
            .pipe(
                finalize(() => {
                    finishedCallback();
                })
            )
            .subscribe((result: ShopProductDtoPagedResultDto) => {
                this.shopProducts = result.items;
                this.showPaging(result, pageNumber);
            });
    }

    delete(shopProduct: ShopProductDto): void {
        abp.message.confirm(
            this.l('ShopProductDeleteWarningMessage', shopProduct.name),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._shopProductService
                        .delete(shopProduct.id)
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

    createShopProduct(): void {
        this.showCreateOrEditShopProductDialog();
    }

    editShopProduct(shopProduct: ShopProductDto): void {
        this.showCreateOrEditShopProductDialog(shopProduct.id);
    }

    showCreateOrEditShopProductDialog(id?: number): void {
        let createOrEditShopProductDialog;
        if (id === undefined || id <= 0) {
            createOrEditShopProductDialog = this._dialog.open(CreateShopProductDialogComponent);
        } else {
            createOrEditShopProductDialog = this._dialog.open(EditShopProductDialogComponent, {
                data: id
            });
        }

        createOrEditShopProductDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }
}
