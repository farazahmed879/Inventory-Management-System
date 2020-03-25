import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    SubTypeDto, SubTypeServiceServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-subType-dialog.component.html',
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
export class EditSubTypeDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  subType: SubTypeDto = new SubTypeDto();

  constructor(
    injector: Injector,
    public _subTypeService: SubTypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<EditSubTypeDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._subTypeService.getById(this._id).subscribe((result: SubTypeDto) => {
      this.subType = result;
    });
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
