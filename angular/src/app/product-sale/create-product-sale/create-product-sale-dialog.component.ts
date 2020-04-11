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

interface Status {
  name: string;
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


  constructor(
    injector: Injector,
    public _productSaleService: ProductSaleServiceServiceProxy,
    public _shopProductService: ShopProductServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateProductSaleDialogComponent>
  ) {
    super(injector);

    this.statuses = [
      { name: 'Sold', value: 'Sold' },
      { name: 'Return', value: 'Return' },
    ];

  }

  ngOnInit(): void {
    this.getAllProduct();
    this.productSale.quantity = 1;
  }

  getAllProduct() {
    this._shopProductService.getAll(this.appSession.tenantId).subscribe(result => {
      this.products = result;
      console.log("shop products",result);
    });
  }

  save(): void {
    this.saving = true;
    this.productSale.tenantId = this.appSession.tenantId;

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
