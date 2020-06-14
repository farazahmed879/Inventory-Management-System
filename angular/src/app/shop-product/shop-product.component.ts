import {Component, Injector, ViewEncapsulation} from "@angular/core";
import {MatDialog} from "@angular/material";
import {finalize} from "rxjs/operators";
import {appModuleAnimation} from "@shared/animations/routerTransition";
import {
    PagedListingComponentBase,
    PagedRequestDto
} from "shared/paged-listing-component-base";
import {
    ShopProductServiceServiceProxy,
    ShopProductDto,
    ShopProductDtoPagedResultDto,
    ProductServiceServiceProxy,
    ProductDto,
    CompanyDto,
    CompanyServiceServiceProxy,
    SubTypeDto,
    SubTypeServiceServiceProxy
} from "@shared/service-proxies/service-proxies";
import {CreateShopProductDialogComponent} from "./create-shop-product/create-shop-product-dialog.component";
import {EditShopProductDialogComponent} from "./edit-shop-product/edit-shop-product-dialog.component";
import {DetailShopProductDialogComponent} from "./detail-shop-product/detail-shop-product-dialog.component";
import {timingSafeEqual} from "crypto";
import {PrimefacesDropDownObject} from "@app/layout/topbar.component";
import {DropdownModule} from 'primeng/dropdown';
import {SelectItem} from 'primeng/api';
import {SelectItemGroup} from 'primeng/api';

class PagedShopProductRequestDto extends PagedRequestDto {
    keyword: string;
}

@Component({
    templateUrl: "./shop-product.component.html",
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
    keyword = "";
    shopProductsFilter: ShopProductDto[] = [];
    selectedProduct: number;
    companies: CompanyDto[] = [];
    selectedCompany: any;
    selectedType: number;
    subTypes: SubTypeDto[] = [];
    selectedSubType: number;
    barcode: any;
    showBarCode: boolean;
    companyArrayObj: PrimefacesDropDownObject[];
    subTypeArrayObj: PrimefacesDropDownObject[];
    request: PagedShopProductRequestDto;

    items: SelectItem[];

    item: string;

    constructor(
        injector: Injector,
        private _shopProductService: ShopProductServiceServiceProxy,
        private _dialog: MatDialog,
        private _productService: ProductServiceServiceProxy,
        private _companyService: CompanyServiceServiceProxy,
        private _subTypeService: SubTypeServiceServiceProxy
    ) {
        super(injector);


    }

    ngOnInit() {
        this.getDataPage(1);
        this.getAllShopProducts();
        this.getCompanies();
        this.getSubTypes();
    }

    getAllShopProducts() {
        this._shopProductService.getAll(this.appSession.tenantId).subscribe(result => {
            this.shopProductsFilter = result;
        });
    }

    getCompanies() {
        this._companyService.getAll(this.appSession.tenantId).subscribe(result => {
            this.companies = result;
            this.companyArrayObj = result.map(item =>
                ({
                    label: item.name,
                    value: item.id
                }));
        });
    }

    getSubTypes() {
        this._subTypeService.getAll(this.appSession.tenantId).subscribe(result => {
            this.subTypes = result;
            this.subTypeArrayObj = result.map(item =>
                ({
                    label: item.name,
                    value: item.id
                }));
        });
    }

    list(
        request: PagedShopProductRequestDto,
        pageNumber: number,
        finishedCallback: Function
    ) {
        this.selectedCompany;
        debugger;
        request.keyword = this.keyword;
        this._shopProductService
            .getPaginatedAll(
                this.keyword,
                this.selectedCompany ? this.selectedCompany.value : undefined,
                this.selectedType,
                this.selectedSubType,
                this.appSession.tenantId,
                request.skipCount,
                request.maxResultCount
            )
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
            this.l("ShopProductDeleteWarningMessage", shopProduct.productName),
            undefined,
            (result: boolean) => {
                if (result) {
                    this._shopProductService
                        .delete(shopProduct.id)
                        .pipe(
                            finalize(() => {
                                abp.notify.success(this.l("SuccessfullyDeleted"));
                                this.refresh();
                            })
                        )
                        .subscribe(() => {
                        });
                }
            }
        );
    }

    generateBarCode(shopProduct: ShopProductDto) {
        this.showDetailShopProductDialog(shopProduct.id);
    }

    showDetailShopProductDialog(id?: number) {
        let detailShopProductDialog;
        if (id === undefined || id <= 0) {
            detailShopProductDialog = this._dialog.open(
                DetailShopProductDialogComponent
            );
        } else {
            detailShopProductDialog = this._dialog.open(
                DetailShopProductDialogComponent,
                {
                    data: id
                }
            );
        }

        detailShopProductDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
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
            createOrEditShopProductDialog = this._dialog.open(
                CreateShopProductDialogComponent
            );
        } else {
            createOrEditShopProductDialog = this._dialog.open(
                EditShopProductDialogComponent,
                {
                    data: id
                }
            );
        }

        createOrEditShopProductDialog.afterClosed().subscribe(result => {
            if (result) {
                this.refresh();
            }
        });
    }

    clearFilters() {
        this.selectedSubType = null;
        this.selectedType = null;
        this.selectedCompany = null;
        this.keyword = "";
    }
}
