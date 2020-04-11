import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef } from '@angular/material';
import { finalize } from 'rxjs/operators';
import { AppComponentBase } from '@shared/app-component-base';
import {
  SubTypeServiceServiceProxy,
  TypeServiceServiceProxy,
  CreateSubTypeDto,
  TypeDto
} from '@shared/service-proxies/service-proxies';
//import { SubTypeTypeLookupTableModalComponent } from '../subType-type-lookup-modal/subType-type-lookup-table-modal.component';
import { ModalDirective } from 'ngx-bootstrap';
import { SelectItem } from 'primeng/api/primeng-api';
import { List } from 'lodash';



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
  type = '';
  types: TypeDto[];

  //@ViewChild('subTypeTypeLookupTableModalComponent', { static: true }) subTypeTypeLookupTableModalComponent: SubTypeTypeLookupTableModalComponent;
  //@ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;

  constructor(
    injector: Injector,

    public _subTypeService: SubTypeServiceServiceProxy,
    public _typeService: TypeServiceServiceProxy,
    private _dialogRef: MatDialogRef<CreateSubTypeDialogComponent>
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getAllProductType();
  }

  getAllProductType() {
    this._typeService.getAll(this.appSession.tenantId).subscribe(result => {
      this.types = result;
    });
  }

  save(): void {
    this.saving = true;
    this.subType.tenantId = this.appSession.tenantId;


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

  getNewTypeId() {
    // this.question.questionTypeId = this.questionQuestionTypeLookupTableModal.id;
    // this.questionTypeType = this.questionQuestionTypeLookupTableModal.displayName;
  }
  openSelectTypeModal() {
    //this.subTypeTypeLookupTableModalComponent.id = this.subType.typeId;
    // this.subTypeTypeLookupTableModalComponent.displayName = this.subType.name;
    //this.subTypeTypeLookupTableModalComponent.show();
  }

  close(result: any): void {
    this._dialogRef.close(result);
  }
}
