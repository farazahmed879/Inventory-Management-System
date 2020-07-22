import { Component, Injector, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
  CompanyDto, CompanyServiceServiceProxy
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-company-dialog.component.html',
  styles: ['./edit-company-dialog.component.less'
  ]
})
export class EditCompanyDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  company: CompanyDto = new CompanyDto();

  constructor(
    injector: Injector,
    public _companyService: CompanyServiceServiceProxy,
    private _dialogRef: MatDialogRef<EditCompanyDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) private _id: number
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._companyService.getById(this._id).subscribe((result: CompanyDto) => {
      this.company = result;
    });
  }

  save(): void {
    this.saving = true;
    this.company.tenantId = this.appSession.tenantId;
    this._companyService
      .createOrEdit(this.company)
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
