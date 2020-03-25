import { Component, Injector,OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ProductSellServiceServiceProxy,
    CreateProductSellDto,
} from '@shared/service-proxies/service-proxies';

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
  productSell: CreateProductSellDto = new CreateProductSellDto();

  constructor(
    injector: Injector,
    public _productSellService: ProductSellServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateProductSellDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
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
