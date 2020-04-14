import { Component, Injector, ViewEncapsulation, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { SideBarNavComponent } from './sidebar-nav.component';

@Component({
    templateUrl: './topbar.component.html',
    selector: 'top-bar',
    encapsulation: ViewEncapsulation.None
})
export class TopBarComponent extends AppComponentBase {

    //@ViewChild('sidebar') sidebar: SideBarNavComponent;
    constructor(
        injector: Injector
    ) {
        super(injector);
    }

    // closeClick() {
    //     this.sidebar.hide();
    // }
}
