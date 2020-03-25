import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ProductSellDto, ProductSellServiceServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-product-sell-dialog.component.html',
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
export class EditProductSellDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  productSell: ProductSellDto = new ProductSellDto();

  constructor(
    injector: Injector,
    public _productSellService: ProductSellServiceServiceProxy,
    private _dialogRef: MatDialogRef<EditProductSellDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._productSellService.getById(this._id).subscribe((result: ProductSellDto) => {
      this.productSell = result;
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
