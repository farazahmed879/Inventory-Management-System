import { Component, Injector } from "@angular/core";
import { MatDialog } from "@angular/material";
import { finalize } from "rxjs/operators";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  PagedListingComponentBase,
  PagedRequestDto
} from "shared/paged-listing-component-base";
import {
  SubTypeServiceServiceProxy,
  SubTypeDto,
  SubTypeDtoPagedResultDto,
  TypeDto,
  TypeServiceServiceProxy
} from "@shared/service-proxies/service-proxies";
import { CreateSubTypeDialogComponent } from "./create-subType/create-subType-dialog.component";
import { EditSubTypeDialogComponent } from "./edit-subType/edit-subType-dialog.component";

class PagedSubTypeRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  templateUrl: "./subType.component.html",
  animations: [appModuleAnimation()],
  styles: [
    `
      mat-form-field {
        padding: 10px;
      }
    `
  ]
})
export class SubTypeComponent extends PagedListingComponentBase<SubTypeDto> {
  subTypes: SubTypeDto[] = [];
  keyword = "";
  types: TypeDto[] = [];
  selectedType: string;

  constructor(
    injector: Injector,
    private _subTypeService: SubTypeServiceServiceProxy,
    private _dialog: MatDialog,
    private _typeService: TypeServiceServiceProxy
  ) {
    super(injector);
  }
  ngOnInit() {
    this.getTypes();
    this.list(undefined, 1, undefined);
  }

  list(
    request: PagedSubTypeRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._subTypeService
      .getPaginatedAll(
        request.keyword,
        this.selectedType,
        this.appSession.tenantId,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: SubTypeDtoPagedResultDto) => {
        this.subTypes = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  clearFilters() {
    this.keyword = "";
    this.selectedType = "";
  }

  delete(subType: SubTypeDto): void {
    abp.message.confirm(
      this.l("SubTypeDeleteWarningMessage", subType.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._subTypeService.delete(subType.id);
          // this._subTypeService
          //   .getPaginatedAll(
          //     request.keyword,
          //     undefined,
          //     request.skipCount,
          //     request.maxResultCount
          //   )
          //   .pipe(
          //     finalize(() => {
          //       abp.notify.success(this.l("SuccessfullyDeleted"));
          //       this.refresh();
          //     })
          //   )
          //   .subscribe(() => {});
        }
      }
    );
  }

  getTypes() {
    this._typeService.getAll(this.appSession.tenantId).subscribe(result => {
      this.types = result;
    });
    console.log(this.types);
  }

  createSubType(): void {
    this.showCreateOrEditSubTypeDialog();
  }

  editSubType(subType: SubTypeDto): void {
    this.showCreateOrEditSubTypeDialog(subType.id);
  }

  showCreateOrEditSubTypeDialog(id?: number): void {
    let createOrEditSubTypeDialog;
    if (id === undefined || id <= 0) {
      createOrEditSubTypeDialog = this._dialog.open(
        CreateSubTypeDialogComponent
      );
    } else {
      createOrEditSubTypeDialog = this._dialog.open(
        EditSubTypeDialogComponent,
        {
          data: id
        }
      );
    }

    createOrEditSubTypeDialog.afterClosed().subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }
}
