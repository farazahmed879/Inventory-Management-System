import { Component, Injector,OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    SubTypeServiceServiceProxy,
    CreateSubTypeDto,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'create-subType-dialog.component.html',
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
export class CreateSubTypeDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  subType: CreateSubTypeDto = new CreateSubTypeDto();

  constructor(
    injector: Injector,
    public _subTypeService: SubTypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateSubTypeDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
  }

  save(): void {
    this.saving = true;

    this._subTypeService
      .createOrEdit(this.subType)
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
