import { Component, Injector,OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ProductServiceServiceProxy,
    CreateProductDto,
    SubTypeServiceServiceProxy,
    SubTypeDto
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'create-product-dialog.component.html',
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
export class CreateProductDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  product: CreateProductDto = new CreateProductDto();
  subTypes: SubTypeDto[];

  constructor(
    injector: Injector,
    public _productService: ProductServiceServiceProxy,
    public _subTYpeService: SubTypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateProductDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllProductType();
  }

  getAllProductType() {
    this._subTYpeService.getAll(this.appSession.tenantId).subscribe(result => {
      this.subTypes = result;
      console.log("subTypes",this.subTypes);
    });
  }
  save(): void {
    this.saving = true;
    this.product.tenantId = this.appSession.tenantId;

    this._productService
      .createOrEdit(this.product)
      .pipe(
        finalize(() => {
          this.saving = false;
        })
      )
      .subscribe(() => {
        this.notify.success(this.l('SavedSuccessfully'));
        this.close(true);
      });
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
