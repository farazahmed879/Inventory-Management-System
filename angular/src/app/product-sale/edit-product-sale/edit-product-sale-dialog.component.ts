import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ProductSaleDto, ProductSaleServiceServiceProxy, ShopProductDto, ShopProductServiceServiceProxy
} from '@shared/service-proxies/service-proxies';

interface Status {
  name: string;
  value: string;
}
@Component({
  templateUrl: 'edit-product-sale-dialog.component.html',
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
export class EditProductSaleDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  productSale: ProductSaleDto = new ProductSaleDto();
  statuses: Status[];
  products: ShopProductDto[];

  constructor(
    injector: Injector,
    public _productSaleService: ProductSaleServiceServiceProxy,
    private _dialogRef: MatDialogRef<EditProductSaleDialogComponent>,
    public _shopProductService: ShopProductServiceServiceProxy,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
  ) {
    super(injector);

    this.statuses = [
      { name: 'Sold', value: 'Sold' },
      { name: 'Return', value: 'Return' },
    ];
  }

  ngOnInit(): void {
    this._productSaleService.getById(this._id).subscribe((result: ProductSaleDto) => {
      this.productSale = result;
    });
    this.getAllProduct();
  }

  getAllProduct() {
    this._shopProductService.getAll(this.appSession.tenantId).subscribe(result => {
      this.products = result;
      console.log("shop products",result);
    });
  }

  save(): void {
    this.saving = true;

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
