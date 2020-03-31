import { Component, Injector, AfterViewInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { ProductSaleServiceServiceProxy } from '@shared/service-proxies/service-proxies';
import { AppConsts } from '@shared/AppConsts';


@Component({
    templateUrl: './sale-dashboard.component.html',
    styleUrls: [
        './sale-dashboard.component.less'
    ],
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
        public _productSaleService: ProductSaleServiceServiceProxy,
    ) {
        super(injector);
        this.getThisWeekGraphData();
    }
    ngOnInIt() {
        //this.graphData();
    }
    getGraphData(type: string,label: string) {
        debugger;
        this.data = {
            // labels: ['January', 'February', 'March', 'April', 'May', 'June', 'July'],
            labels: [],
            datasets: [
                {
                    label: label,
                    backgroundColor: '#42A5F5',
                    borderColor: '#1E88E5',
                    data: []
                },
                {
                    label: 'Profit',
                    backgroundColor: '#9CCC65',
                    borderColor: '#7CB342',
                    data: []
                },
                {
                    label: 'Product Costing',
                    backgroundColor: '	#FF6347 ',
                    borderColor: '#800000',
                    data: []
                },
                {
                    label: 'Other Sale',
                    backgroundColor: '	#FF0000 ',
                    borderColor: '#8B0000',
                    data: []
                }
            ]
        }

        this._productSaleService.getAllProductSale(type,undefined).subscribe(result => {
            console.log("Graph Data", result);
            for (var x = 0; x < result.length; x++) {
                var day = result[x].label;
                var sale = result[x].sale;
                var profit = result[x].profit;
                var expense = result[x].expense;
                var productCost = result[x].productCost;
                this.data.labels.push(day);
                this.data.datasets[0].data.push(sale);
                this.data.datasets[1].data.push(profit);
                this.data.datasets[2].data.push(productCost);
                this.data.datasets[3].data.push(expense);
                
            }
            this.viewGraph = true;
            //this.weeks = result.label;
        });


    }

  
    getThisWeekGraphData(){
        this.viewGraph = false;
        var label = "Current week sale";
        this.getGraphData(AppConsts.graphData.ThisWeek,label);
    }
    getThisMonthGraphData(){
        this.viewGraph = false;
        var label = "Current month sale";
        this.getGraphData(AppConsts.graphData.ThisMonth,label);
    }
    getThisYearGraphData(){
        this.viewGraph = false;
        var label = "Current year sale";
        this.getGraphData(AppConsts.graphData.ThisYear,label);
    }
    getAllYearGraphData(){
        this.viewGraph = false;
        var label = "All year sale";
        this.getGraphData(AppConsts.graphData.AllYear,label);
    }

    ngAfterViewInit(): void {


    }
}
