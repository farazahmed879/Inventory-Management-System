import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';

import { ModalModule } from 'ngx-bootstrap';
import { NgxPaginationModule } from 'ngx-pagination';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { AbpModule } from '@abp/abp.module';

import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { SharedModule } from '@shared/shared.module';

import { HomeComponent } from '@app/home/home.component';
import { AboutComponent } from '@app/about/about.component';
import { TopBarComponent } from '@app/layout/topbar.component';
import { TopBarLanguageSwitchComponent } from '@app/layout/topbar-languageswitch.component';
import { SideBarUserAreaComponent } from '@app/layout/sidebar-user-area.component';
import { SideBarNavComponent } from '@app/layout/sidebar-nav.component';
import { SideBarFooterComponent } from '@app/layout/sidebar-footer.component';
import { RightSideBarComponent } from '@app/layout/right-sidebar.component';
// tenants
import { TenantsComponent } from '@app/tenants/tenants.component';
import { CreateTenantDialogComponent } from './tenants/create-tenant/create-tenant-dialog.component';
import { EditTenantDialogComponent } from './tenants/edit-tenant/edit-tenant-dialog.component';
// roles
import { RolesComponent } from '@app/roles/roles.component';
import { CreateRoleDialogComponent } from './roles/create-role/create-role-dialog.component';
import { EditRoleDialogComponent } from './roles/edit-role/edit-role-dialog.component';
// users
import { UsersComponent } from '@app/users/users.component';
import { CreateUserDialogComponent } from '@app/users/create-user/create-user-dialog.component';
import { EditUserDialogComponent } from '@app/users/edit-user/edit-user-dialog.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { ResetPasswordDialogComponent } from './users/reset-password/reset-password.component';
// Type
import { TypeComponent } from '@app/type/type.component';
import { CreateTypeDialogComponent } from '@app/type/create-type/create-type-dialog.component';
import { EditTypeDialogComponent } from '@app/type/edit-type/edit-type-dialog.component';
// Product
import { ProductComponent } from '@app/product/product.component';
import { CreateProductDialogComponent } from '@app/product/create-product/create-product-dialog.component';
import { EditProductDialogComponent } from '@app/product/edit-product/edit-product-dialog.component';
// Shop Product
import { ShopProductComponent } from './shop-product/shop-product.component';
import { EditShopProductDialogComponent } from './shop-product/edit-shop-product/edit-shop-product-dialog.component';
import { CreateShopProductDialogComponent } from './shop-product/create-shop-product/create-shop-product-dialog.component';
// Product Sell
import { ProductSellComponent } from './product-sell/product-sell.component';
import { CreateProductSellDialogComponent } from './product-sell/create-product-sell/create-product-sell-dialog.component';
import { EditProductSellDialogComponent } from './product-sell/edit-product-sell/edit-product-sell-dialog.component';
// Company
import { CompanyComponent } from './company/company.component';
import { CreateCompanyDialogComponent } from './company/create-company/create-company-dialog.component';
import { EditCompanyDialogComponent } from './company/edit-company/edit-company-dialog.component';
// Sub Type
import { SubTypeComponent } from './subType/subType.component';
import { CreateSubTypeDialogComponent } from './subType/create-subType/create-subType-dialog.component';
import { EditSubTypeDialogComponent } from './subType/edit-subType/edit-subType-dialog.component';
import { SubTypeTypeLookupTableModalComponent } from './subType/subType-type-lookup-modal/subType-type-lookup-table-modal.component';
//PrimeNg

import {DropdownModule} from 'primeng/dropdown';
//Services
import {
  ShopProductServiceServiceProxy,
  SubTypeServiceServiceProxy,
  ProductServiceServiceProxy,
  CompanyServiceServiceProxy,
  TypeServiceServiceProxy,
  ProductSellServiceServiceProxy
} from '@shared/service-proxies/service-proxies';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AboutComponent,
    TopBarComponent,
    TopBarLanguageSwitchComponent,
    SideBarUserAreaComponent,
    SideBarNavComponent,
    SideBarFooterComponent,
    RightSideBarComponent,
    // tenants
    TenantsComponent,
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    RolesComponent,
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    // users
    UsersComponent,
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ChangePasswordComponent,
    ResetPasswordDialogComponent,
    // type
    TypeComponent,
    CreateTypeDialogComponent,
    EditTypeDialogComponent,
    //SubType
    SubTypeComponent,
    CreateSubTypeDialogComponent,
    EditSubTypeDialogComponent,
    //SubTypeTypeLookupTableModalComponent,
    //Product
    ProductComponent,
    CreateProductDialogComponent,
    EditProductDialogComponent,
    //ShopProduct
    ShopProductComponent,
    CreateShopProductDialogComponent,
    EditShopProductDialogComponent,
    //ProductSell
    ProductSellComponent,
    CreateProductSellDialogComponent,
    EditProductSellDialogComponent,
    //Company
    CompanyComponent,
    CreateCompanyDialogComponent,
    EditCompanyDialogComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    HttpClientJsonpModule,
    ModalModule.forRoot(),
    AbpModule,
    AppRoutingModule,
    ServiceProxyModule,
    SharedModule,
    NgxPaginationModule,
    DropdownModule
  ],
  providers: [
    ShopProductServiceServiceProxy,
    CompanyServiceServiceProxy,
    TypeServiceServiceProxy,
    SubTypeServiceServiceProxy,
    ProductServiceServiceProxy,
    ProductSellServiceServiceProxy,
  ],
  entryComponents: [
    // tenants
    CreateTenantDialogComponent,
    EditTenantDialogComponent,
    // roles
    CreateRoleDialogComponent,
    EditRoleDialogComponent,
    // users
    CreateUserDialogComponent,
    EditUserDialogComponent,
    ResetPasswordDialogComponent,
    //Type
    TypeComponent,
    CreateTypeDialogComponent,
    EditTypeDialogComponent,
    //SubType
    SubTypeComponent,
    CreateSubTypeDialogComponent,
    EditSubTypeDialogComponent,
    //Product
    ProductComponent,
    CreateProductDialogComponent,
    EditProductDialogComponent,
    //ShopProduct
    ShopProductComponent,
    CreateShopProductDialogComponent,
    EditShopProductDialogComponent,
    //ProductSell
    ProductSellComponent,
    CreateProductSellDialogComponent,
    EditProductSellDialogComponent,
    //Company
    CompanyComponent,
    CreateCompanyDialogComponent,
    EditCompanyDialogComponent
  ]
})
export class AppModule { }
