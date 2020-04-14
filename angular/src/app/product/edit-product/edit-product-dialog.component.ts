import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    ProductDto, 
    ProductServiceServiceProxy,
    SubTypeServiceServiceProxy,
    SubTypeDto
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-product-dialog.component.html',
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
export class EditProductDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  product: ProductDto = new ProductDto();
  subTypes: SubTypeDto[];

  constructor(
    injector: Injector,
    public _productService: ProductServiceServiceProxy,
    public _subTypeService: SubTypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<EditProductDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._productService.getById(this._id).subscribe((result: ProductDto) => {
      this.product = result;
    });
    this.getAllProductType();
  }
  getAllProductType() {
    this._subTypeService.getAll(this.appSession.tenantId).subscribe(result => {
      this.subTypes = result;
    });
  }
  save(): void {
    this.saving = true;

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
