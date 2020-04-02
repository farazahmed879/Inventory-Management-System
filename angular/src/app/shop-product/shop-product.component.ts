import { Component, Injector } from "@angular/core";
import { MatDialog } from "@angular/material";
import { finalize } from "rxjs/operators";
import { appModuleAnimation } from "@shared/animations/routerTransition";
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
  TypeDto,
  TypeServiceServiceProxy,
  SubTypeDto,
  SubTypeServiceServiceProxy
} from "@shared/service-proxies/service-proxies";
import { CreateShopProductDialogComponent } from "./create-shop-product/create-shop-product-dialog.component";
import { EditShopProductDialogComponent } from "./edit-shop-product/edit-shop-product-dialog.component";

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
export class ShopProductComponent extends PagedListingComponentBase<
  ShopProductDto
> {
  shopProducts: ShopProductDto[] = [];
  keyword = "";
  shopProductsFilter: ShopProductDto[] = [];
  selectedProduct: number;
  companies: CompanyDto[] = [];
  selectedCompany: number;
  types: TypeDto[] = [];
  selectedType: number;
  subTypes: SubTypeDto[] = [];
  selectedSubType: number;

  constructor(
    injector: Injector,
    private _shopProductService: ShopProductServiceServiceProxy,
    private _dialog: MatDialog,
    private _productService: ProductServiceServiceProxy,
    private _companyService: CompanyServiceServiceProxy,
    private _typeService: TypeServiceServiceProxy,
    private _subTypeService: SubTypeServiceServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getAllShopProducts();
    this.getCompanies();
    this.getTypes();
    this.getSubTypes();
  }

  getAllShopProducts() {
    this._shopProductService.getAll().subscribe(result => {
      this.shopProductsFilter = result;
    });
  }

  getCompanies() {
    this._companyService.getAll().subscribe(result => {
      this.companies = result;
    });
  }

  getTypes() {
    this._typeService.getAll().subscribe(result => {
      this.types = result;
    });
  }

  getSubTypes() {
    this._subTypeService.getAll().subscribe(result => {
      this.subTypes = result;
    });
  }

  list(
    request: PagedShopProductRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._shopProductService
      .getPaginatedAll(
        request.keyword,
        this.selectedCompany,
        this.selectedType,
        this.selectedSubType,
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
            .subscribe(() => {});
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

  clearFilters(){
    this.selectedSubType = null;
    this.selectedType = null;
    this.selectedCompany = null;
    this.keyword = "";
  }
}
