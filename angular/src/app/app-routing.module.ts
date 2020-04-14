import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { HomeComponent } from './home/home.component';
import { AboutComponent } from './about/about.component';
import { UsersComponent } from './users/users.component';
import { TenantsComponent } from './tenants/tenants.component';
import { RolesComponent } from 'app/roles/roles.component';
import { ChangePasswordComponent } from './users/change-password/change-password.component';
import { TypeComponent } from './type/type.component';
import { ProductComponent } from './product/product.component';
import { CompanyComponent } from './company/company.component';
import { ProductSaleComponent } from './product-sale/product-sale.component';
import { ShopProductComponent } from './shop-product/shop-product.component';
import { SubTypeComponent } from './subType/subType.component';
import { SaleDashboardComponent } from './sale-dashboard/sale-dashboard.component';
import { ExpenseComponent } from './expense/expense.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
                    { path: 'dashboard', component: SaleDashboardComponent,  canActivate: [AppRouteGuard] },
                    { path: 'expense', component: ExpenseComponent,  canActivate: [AppRouteGuard] },
                    { path: 'users', component: UsersComponent, data: { permission: 'Pages.Users' }, canActivate: [AppRouteGuard] },
                    { path: 'type', component: TypeComponent, canActivate: [AppRouteGuard] },
                    { path: 'product', component: ProductComponent, canActivate: [AppRouteGuard] },
                    { path: 'company', component: CompanyComponent, canActivate: [AppRouteGuard] },
                    { path: 'productSale', component: ProductSaleComponent, canActivate: [AppRouteGuard] },
                    { path: 'shopProduct', component: ShopProductComponent, canActivate: [AppRouteGuard] },
                    { path: 'productCatagory', component: SubTypeComponent,canActivate: [AppRouteGuard] },
                    { path: 'roles', component: RolesComponent, data: { permission: 'Pages.Roles' }, canActivate: [AppRouteGuard] },
                    { path: 'tenants', component: TenantsComponent, data: { permission: 'Pages.Tenants' }, canActivate: [AppRouteGuard] },
                    { path: 'about', component: AboutComponent },
                    { path: 'update-password', component: ChangePasswordComponent }
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }
