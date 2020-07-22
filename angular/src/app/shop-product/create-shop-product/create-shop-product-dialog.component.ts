import { Component, Injector, OnInit } from "@angular/core";
import { MatDialogRef } from "@angular/material";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/app-component-base";
import {
  ShopProductServiceServiceProxy,
  CreateShopProductDto,
  CompanyServiceServiceProxy,
  CompanyDto,
  ProductServiceServiceProxy,
  ProductDto,
  SubTypeServiceServiceProxy,
  SubTypeDto,
} from "@shared/service-proxies/service-proxies";
import { PrimefacesDropDownObject } from "@app/layout/topbar.component";

@Component({
  templateUrl: "create-shop-product-dialog.component.html",
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
export class CreateShopProductDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  shopProduct: CreateShopProductDto = new CreateShopProductDto();
  companies: CompanyDto[];
  products: ProductDto[];
  subTypes: SubTypeDto[] = [];
  selectedSubTypeId: string;
  selectedTypeId: string;
  companyArrayObj: PrimefacesDropDownObject[];
  productArrayObj: PrimefacesDropDownObject[];
  selectedCompanyObj: PrimefacesDropDownObject;
  selectedProductObj: PrimefacesDropDownObject;

  constructor(
    injector: Injector,
    public _shopProductService: ShopProductServiceServiceProxy,
    public _productService: ProductServiceServiceProxy,
    public _companyService: CompanyServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateShopProductDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllProduct();
    this.getAllCompany();
  }

  getAllCompany() {
    this._companyService.getAll(this.appSession.tenantId).subscribe(result => {
      this.companies = result;
      this.companyArrayObj = result.map(item =>
        ({
            label: item.name,
            value: item.id
        }));
    });
  }

  getAllProduct() {
    this._productService.getAll(this.appSession.tenantId).subscribe(result => {
      this.products = result;
      this.productArrayObj = result.map(item =>
        ({
            label: item.name,
            value: item.id
        }));
    });
  }

  save(): void {
    this.saving = true;
    this.shopProduct.tenantId = this.appSession.tenantId;
    this.shopProduct.companyId =  this.selectedCompanyObj.value;
    this.shopProduct.productId = this.selectedProductObj.value;
    this._shopProductService
      .createOrEdit(this.shopProduct)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l("SavedSuccessfully"));
        this.close(true);
      });
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
