import { Component, Injector, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
  ProductSaleServiceServiceProxy,
  CreateProductSaleDto,
  ProductDto,
  ShopProductServiceServiceProxy,
  ShopProductDto,
} from '@shared/service-proxies/service-proxies';
import { PrimefacesDropDownObject } from '@app/layout/topbar.component';

interface Status {
  label: string;
  value: string;
}

@Component({
  templateUrl: 'create-product-sale-dialog.component.html',
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
export class CreateProductSaleDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  productSale: CreateProductSaleDto = new CreateProductSaleDto();
  statuses: Status[];
  products: ShopProductDto[];
  productArrayObj : PrimefacesDropDownObject[];
  selectedProductObj: PrimefacesDropDownObject;
  selectedStatusObj: any;
  constructor(
    injector: Injector,
    public _productSaleService: ProductSaleServiceServiceProxy,
    public _shopProductService: ShopProductServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateProductSaleDialogComponent>
  ) {
    super(injector);

    this.statuses = [
      { label: 'Sold', value: 'Sold' },
      { label: 'Return', value: 'Return' },
    ];

  }

  ngOnInit(): void {
    this.getAllProduct();
    this.productSale.quantity = 1;
  }

  getAllProduct() {
    this._shopProductService.getAll(this.appSession.tenantId).subscribe(result => {
      this.products = result;
      this.productArrayObj = result.map(item =>
        ({
            label: item.productName,
            value: item.id
        }));
      console.log("shop products",result);
    });
  }

  save(): void {
    this.saving = true;
    this.productSale.tenantId = this.appSession.tenantId;
    this.productSale.shopProductId = this.selectedProductObj.value;
    this.productSale.status = this.selectedStatusObj.value;
    this._productSaleService
      .createOrEdit(this.productSale)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.close(true);
      });
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
