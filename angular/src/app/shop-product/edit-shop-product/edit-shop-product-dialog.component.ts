import { Component, Injector, OnInit, Inject, Optional } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/app-component-base";
import {
  ShopProductDto,
  ShopProductServiceServiceProxy,
  ProductServiceServiceProxy,
  CompanyServiceServiceProxy,
  CompanyDto,
  ProductDto,
  SubTypeServiceServiceProxy,
  SubTypeDto,
} from "@shared/service-proxies/service-proxies";
import { PrimefacesDropDownObject } from "@app/layout/topbar.component";

@Component({
  templateUrl: "edit-shop-product-dialog.component.html",
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
export class EditShopProductDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  shopProduct: ShopProductDto = new ShopProductDto();
  companies: CompanyDto[];
  products: ProductDto[];
  subTypes: SubTypeDto[] = [];
  selectedTypeId: string;
  selectedSubTypeId: string;
  companyArrayObj: PrimefacesDropDownObject[];
  productArrayObj: PrimefacesDropDownObject[];
  selectedCompanyObj: PrimefacesDropDownObject;
  selectedProductObj: PrimefacesDropDownObject;

  constructor(
    injector: Injector,
    public _shopProductService: ShopProductServiceServiceProxy,
    public _productService: ProductServiceServiceProxy,
    public _companyService: CompanyServiceServiceProxy,
    private _subTypeService: SubTypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<EditShopProductDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._shopProductService
      .getById(this._id)
      .subscribe((result: ShopProductDto) => {
        this.shopProduct = result;
      });
    this.getAllCompany();
    this.getAllProduct();
  }
  getAllCompany() {
    this._companyService.getAll(this.appSession.tenantId).subscribe(result => {
      this.companies = result;
      this.companyArrayObj = result.map(item =>
        ({
            label: item.name,
            value: item.id
        }));
        this.selectedCompanyObj = this.companyArrayObj.filter(i=> i.value == this.shopProduct.companyId)[0];
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
        this.selectedProductObj = this.productArrayObj.filter(i=> i.value == this.shopProduct.productId)[0];
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
