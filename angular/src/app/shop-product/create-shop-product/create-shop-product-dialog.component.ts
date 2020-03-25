import { Component, Injector,OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ShopProductServiceServiceProxy,
    CreateShopProductDto,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'create-shop-product-dialog.component.html',
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

  constructor(
    injector: Injector,
    public _shopProductService: ShopProductServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateShopProductDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
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
        this.notify.info(this.l('SavedSuccessfully'));
        this.close(true);
      });
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
