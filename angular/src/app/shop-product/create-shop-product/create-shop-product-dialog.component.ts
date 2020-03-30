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
  TypeServiceServiceProxy,
  SubTypeServiceServiceProxy,
  SubTypeDto,
  TypeDto
} from "@shared/service-proxies/service-proxies";

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
  types: TypeDto[] = [];
  selectedSubTypeId: string;
  selectedTypeId: string;

  constructor(
    injector: Injector,
    public _shopProductService: ShopProductServiceServiceProxy,
    public _productService: ProductServiceServiceProxy,
    public _companyService: CompanyServiceServiceProxy,
    private _typeService: TypeServiceServiceProxy,
    private _subTypeService: SubTypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateShopProductDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllProduct();
    this.getAllCompany();
    this.getAllSubTypes();
    this.getAllTypes();
  }

  getAllCompany() {
    this._companyService.getAll().subscribe(result => {
      this.companies = result;
    });
  }

  getAllProduct() {
    this._productService.getAll().subscribe(result => {
      this.products = result;
    });
  }

  getAllTypes() {
    this._typeService.getAll().subscribe(result => {
      this.types = result;
    });
  }

  getAllSubTypes() {
    this._subTypeService.getAll().subscribe(result => {
      this.subTypes = result;
    });
  }

  save(): void {
    this.saving = true;

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
