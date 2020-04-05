import { Component, Injector, ViewEncapsulation, Renderer2 } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { MenuItem } from '@shared/layout/menu-item';

@Component({
    templateUrl: './sidebar-nav.component.html',
    selector: 'sidebar-nav',
    encapsulation: ViewEncapsulation.None
})
export class SideBarNavComponent extends AppComponentBase {
    insideTm: any;
    outsideTm: any;
    menuItems: MenuItem[] = [
        // new MenuItem(this.l('HomePage'), '', 'home', '/app/home'), 
        new MenuItem(this.l('Dashboard'), '','home', '/app/dashboard'), 
        new MenuItem(this.l('Product Sale'), '','home', '/app/productSale'), 
        new MenuItem(this.l('Shop Product'), '','business', '/app/shopProduct'),
        new MenuItem(this.l('Expenses'), '','business', '/app/expense'),
        new MenuItem(this.l('Product'), '','business', '/app/product'),
        new MenuItem(this.l('Company'), '','business', '/app/company'),
        new MenuItem(this.l('Product Catagory'), '','business', '/app/productCatagory'),      
        new MenuItem(this.l('Shop'), '','business', '/app/type'),
        new MenuItem(this.l('Tenants'), 'Pages.Tenants', 'business', '/app/tenants'),
        new MenuItem(this.l('Users'), 'Pages.Users', 'people', '/app/users'),
        new MenuItem(this.l('Roles'), 'Pages.Roles', 'local_offer', '/app/roles'),
        new MenuItem(this.l('About'), '', 'info', '/app/about'),

        // new MenuItem(this.l('MultiLevelMenu'), '', 'menu', '', [
        //     new MenuItem('ASP.NET Boilerplate', '', '', '', [
        //         new MenuItem('Home', '', '', 'https://aspnetboilerplate.com/?ref=abptmpl'),
        //         new MenuItem('Templates', '', '', 'https://aspnetboilerplate.com/Templates?ref=abptmpl'),
        //         new MenuItem('Samples', '', '', 'https://aspnetboilerplate.com/Samples?ref=abptmpl'),
        //         new MenuItem('Documents', '', '', 'https://aspnetboilerplate.com/Pages/Documents?ref=abptmpl')
        //     ]),
        //     new MenuItem('ASP.NET Zero', '', '', '', [
        //         new MenuItem('Home', '', '', 'https://aspnetzero.com?ref=abptmpl'),
        //         new MenuItem('Description', '', '', 'https://aspnetzero.com/?ref=abptmpl#description'),
        //         new MenuItem('Features', '', '', 'https://aspnetzero.com/?ref=abptmpl#features'),
        //         new MenuItem('Pricing', '', '', 'https://aspnetzero.com/?ref=abptmpl#pricing'),
        //         new MenuItem('Faq', '', '', 'https://aspnetzero.com/Faq?ref=abptmpl'),
        //         new MenuItem('Documents', '', '', 'https://aspnetzero.com/Documents?ref=abptmpl')
        //     ])
        // ])
    ];

    constructor(
        injector: Injector,
        private render: Renderer2
    ) {
        super(injector);
       // $("#leftsidebar").slideToggle();
    //    setTimeout(()=>{
    //     this.ngOnInIt();
    //    },1000)
      
    }
    // ngOnInIt(){
    //     $("#leftsidebar").slideToggle();
    // }

    showMenuItem(menuItem): boolean {
        if (menuItem.permissionName) {
            return this.permission.isGranted(menuItem.permissionName);
        }

        return true;
    }

    mouseEnter(e: Event) {

       //$("#leftsidebar").toggle("slide");
      // $("#leftsidebar").slideToggle();
      // $("#leftsidebar").slideToggle( { direction: "left" }, 1000);
        // check if the left aside menu is fixed
        //  if (document.body.classList.contains('kt-aside--fixed')) {
        //      if (this.outsideTm) {
        //          clearTimeout(this.outsideTm);
        //          this.outsideTm = null;
        //      }
 
        //      this.insideTm = setTimeout(() => {
        //          // if the left aside menu is minimized
        //          if (document.body.classList.contains('kt-aside--minimize')) {
        //              // show the left aside menu
        //              this.render.removeClass(document.body, 'kt-aside--minimize');
        //              this.render.addClass(document.body, 'kt-aside--minimize-hover');
        //          }
        //      }, 50);
        //  }
     }
 
     mouseLeave(e: Event) {
        //   $("#leftsidebar").slideToggle();
    //      if (document.body.classList.contains('kt-aside--fixed')) {
    //          if (this.insideTm) {
    //              clearTimeout(this.insideTm);
    //              this.insideTm = null;
    //          }
 
    //          this.outsideTm = setTimeout(() => {
    //              // if the left aside menu is expand
    //              if (document.body.classList.contains('kt-aside--minimize-hover') ) {
    //                  // hide back the left aside menu
    //                  this.render.removeClass(document.body, 'kt-aside--minimize-hover');
    //                  this.render.addClass(document.body, 'kt-aside--minimize');
    //              }
    //          }, 100);
    //      }
      }
}
