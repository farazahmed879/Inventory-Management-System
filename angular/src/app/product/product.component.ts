import { Component, Injector } from "@angular/core";
import { MatDialog } from "@angular/material";
import { finalize } from "rxjs/operators";
import { appModuleAnimation } from "@shared/animations/routerTransition";
import {
  PagedListingComponentBase,
  PagedRequestDto
} from "shared/paged-listing-component-base";
import {
  ProductServiceServiceProxy,
  ProductDto,
  ProductDtoPagedResultDto,
  SubTypeDto,
  SubTypeServiceServiceProxy
} from "@shared/service-proxies/service-proxies";
import { CreateProductDialogComponent } from "./create-product/create-product-dialog.component";
import { EditProductDialogComponent } from "./edit-product/edit-product-dialog.component";

class PagedProductRequestDto extends PagedRequestDto {
  keyword: string;
}

@Component({
  templateUrl: "./product.component.html",
  animations: [appModuleAnimation()],
  styles: [
    `
      mat-form-field {
        padding: 10px;
      }
    `
  ]
})
export class ProductComponent extends PagedListingComponentBase<ProductDto> {
  products: ProductDto[] = [];
  keyword = "";
  subTypes: SubTypeDto[] = [];
  selectedSubType: number;

  constructor(
    injector: Injector,
    private _productService: ProductServiceServiceProxy,
    private _dialog: MatDialog,
    private _subTypeService: SubTypeServiceServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    this.getSubtypes();
    this.list(null, 0, undefined);
  }

  list(
    request: PagedProductRequestDto,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.keyword = this.keyword;

    this._productService
      .getPaginatedAll(
        request.keyword,
        this.selectedSubType,
        request.skipCount,
        request.maxResultCount
      )
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: ProductDtoPagedResultDto) => {
        this.products = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  delete(product: ProductDto): void {
    abp.message.confirm(
      this.l("ProductDeleteWarningMessage", product.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._productService
            .delete(product.id)
            .pipe(
              finalize(() => {
                abp.notify.success(this.l("SuccessfullyDeleted"));
                this.refresh();
              })
            )
            .subscribe(() => {});
        }
      }
    );
  }
  clearFilters() {
    this.keyword = "";
    this.selectedSubType = null;
  }

  createProduct(): void {
    this.showCreateOrEditProductDialog();
  }

  getSubtypes() {
    this._subTypeService.getAll().subscribe(result => {
      this.subTypes = result;
    });
  }

  editProduct(product: ProductDto): void {
    this.showCreateOrEditProductDialog(product.id);
  }

  showCreateOrEditProductDialog(id?: number): void {
    let createOrEditProductDialog;
    if (id === undefined || id <= 0) {
      createOrEditProductDialog = this._dialog.open(
        CreateProductDialogComponent
      );
    } else {
      createOrEditProductDialog = this._dialog.open(
        EditProductDialogComponent,
        {
          data: id
        }
      );
    }

    createOrEditProductDialog.afterClosed().subscribe(result => {
      if (result) {
        this.refresh();
      }
    });
  }
}
