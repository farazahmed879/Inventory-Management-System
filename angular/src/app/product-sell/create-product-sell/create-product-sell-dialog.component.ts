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
  templateUrl: 'create-product-sell-dialog.component.html',
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
export class CreateProductSellDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  productSell: CreateProductSaleDto = new CreateProductSaleDto();
  statuses: Status[];
  products: ShopProductDto[];


  constructor(
    injector: Injector,
    public _productSellService: ProductSaleServiceServiceProxy,
    public _shopProductService: ShopProductServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateProductSellDialogComponent>
  ) {
    super(injector);

    this.statuses = [
      { name: 'Sold', value: 'Sold' },
      { name: 'Return', value: 'Return' },
    ];

  }

  ngOnInit(): void {
    this.getAllProduct();
  }

  getAllProduct() {
    this._shopProductService.getAll().subscribe(result => {
      this.products = result;
      console.log("shop products",result);
    });
  }

  save(): void {
    this.saving = true;

    this._productSellService
      .createOrEdit(this.productSell)
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
