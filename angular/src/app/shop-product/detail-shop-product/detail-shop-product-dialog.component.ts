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

@Component({
  templateUrl: "detail-shop-product-dialog.component.html",
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
export class DetailShopProductDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  shopProduct: ShopProductDto = new ShopProductDto();
  companies: CompanyDto[];
  products: ProductDto[];
  subTypes: SubTypeDto[] = [];
  selectedTypeId: string;
  selectedSubTypeId: string;
  barcode: any;
  constructor(
    injector: Injector,
    public _shopProductService: ShopProductServiceServiceProxy,
    public _productService: ProductServiceServiceProxy,
    public _companyService: CompanyServiceServiceProxy,
    private _subTypeService: SubTypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<DetailShopProductDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._shopProductService
      .getById(this._id)
      .subscribe((result: ShopProductDto) => {
        this.shopProduct = result;
        this.barcode = result.creationTime;
      });
  }


  close(result: any): void {
    this._dialogRef.close(result);
  }
}
