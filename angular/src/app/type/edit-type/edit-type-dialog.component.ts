import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
    TypeDto, TypeServiceServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-type-dialog.component.html',
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
export class EditTypeDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  type: TypeDto = new TypeDto();

  constructor(
    injector: Injector,
    public _typeService: TypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<EditTypeDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._typeService.getById(this._id).subscribe((result: TypeDto) => {
      this.type = result;
    });
  }

  save(): void {
    this.saving = true;

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
