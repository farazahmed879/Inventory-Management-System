import { Component, Injector,OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    TypeServiceServiceProxy,
    CreateTypeDto,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'create-type-dialog.component.html',
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
export class CreateTypeDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  type: CreateTypeDto = new CreateTypeDto();

  constructor(
    injector: Injector,
    public _typeService: TypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateTypeDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
  }

  save(): void {
    this.saving = true;
    this.type.tenantId = this.appSession.tenantId;

    this._typeService
      .createOrEdit(this.type)
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
