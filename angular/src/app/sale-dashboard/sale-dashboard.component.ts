import { Component, Injector, AfterViewInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ProductSellServiceServiceProxy } from '@shared/service-proxies/service-proxies';


@Component({
    templateUrl: './sale-dashboard.component.html',
    animations: [appModuleAnimation()]
})
export class SaleDashboardComponent extends AppComponentBase implements AfterViewInit {
    data: any;
    options: any;
    weeks: any;
    values: any;
    viewGraph: boolean = false;
    constructor(
        injector: Injector,
        public _productSaleService: ProductSellServiceServiceProxy,
    ) {
        super(injector);
        this.graphData();
    }
    ngOnInIt() {
        //this.graphData();
    }
    graphData() {
        this.data = {
            // labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
            labels: [],
            datasets: [
                {
                    label: 'My First dataset',
                    backgroundColor: '#42A5F5',
                    borderColor: '#1E88E5',
                    data: []
                }
                // {
                //     label: 'My Second dataset',
                //     backgroundColor: '#9CCC65',
                //     borderColor: '#7CB342',
                //     data: [28, 48, 40, 19, 86, 27, 90]
                // }
            ]
        }

        this._productSaleService.getAllProductSale().subscribe(result => {
            console.log("Graph Data", result);
            for (var x = 0; x < result.length; x++) {
                debugger;
                var day = result[x].label;
                var value = result[x].value;
                this.data.labels.push(day);
                this.data.datasets[0].data.push(value);
            }
            this.viewGraph = true;
            //this.weeks = result.label;
        });


    }
    ngAfterViewInit(): void {


    }
}
