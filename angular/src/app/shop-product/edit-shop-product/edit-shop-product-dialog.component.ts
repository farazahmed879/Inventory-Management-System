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
  TypeServiceServiceProxy,
  SubTypeServiceServiceProxy,
  SubTypeDto,
  TypeDto
} from "@shared/service-proxies/service-proxies";

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
  types: TypeDto[] = [];
  subTypes: SubTypeDto[] = [];
  selectedTypeId: string;
  selectedSubTypeId: string;

  constructor(
    injector: Injector,
    public _shopProductService: ShopProductServiceServiceProxy,
    public _productService: ProductServiceServiceProxy,
    public _companyService: CompanyServiceServiceProxy,
    private _typeService: TypeServiceServiceProxy,
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
    this.getAllTypes();
    this.getAllSubTypes();
  }
  getAllCompany() {
    this._companyService.getAll(this.appSession.tenantId).subscribe(result => {
      this.companies = result;
    });
  }

  getAllProduct() {
    this._productService.getAll(this.appSession.tenantId).subscribe(result => {
      this.products = result;
    });
  }

  getAllTypes() {
    this._typeService.getAll(this.appSession.tenantId).subscribe(result => {
      this.types = result;
    });
  }

  getAllSubTypes() {
    this._subTypeService.getAll(this.appSession.tenantId).subscribe(result => {
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
