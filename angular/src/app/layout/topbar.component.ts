import { Component, Injector, ViewEncapsulation, ViewChild, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { SideBarNavComponent } from './sidebar-nav.component';

@Component({
    templateUrl: './topbar.component.html',
    selector: 'top-bar',
    encapsulation: ViewEncapsulation.None
})
export class TopBarComponent extends AppComponentBase implements OnInit {
    shownLoginName : string;
    //@ViewChild('sidebar') sidebar: SideBarNavComponent;
    constructor(
        injector: Injector
    ) {
        super(injector);
    }
    ngOnInit() {
        
        var name = this.appSession.getShownLoginName();
        const chars = name.split("\\");
        this.shownLoginName = chars[1];
        console.log("shownLoginName", this.shownLoginName);
    }

    // closeClick() {
    //     this.sidebar.hide();
    // }
}

export class PrimefacesDropDownObject {
    label: string;
    value: number;
}
