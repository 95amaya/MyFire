import { Component } from '@angular/core';
import * as Highcharts from 'highcharts'; //TODO: Import Module Types

@Component({
  selector: 'app-home',
  template: `
    <div class="w-full min-h-screen bg-base-200">
      <div class="p-4">
        <div class="card bg-base-100 drop-shadow-xl">
          <div class="card-body">
            <div class="card-title mx-auto text-3xl">Income Statement</div>
            <highcharts-chart
              [Highcharts]="Highcharts"
              [options]="chartOptions"
            >
            </highcharts-chart>
            <button class="btn" (click)="generateNewData()">
              Randomize Data
            </button>
          </div>
        </div>
      </div>
      <div class="p-4">
        <div class="card bg-base-100 drop-shadow-xl">
          <div class="card-body">
            <div class="card-title mx-auto text-3xl">Income Statement</div>
            <highcharts-chart
              [Highcharts]="Highcharts"
              [options]="lineChartOptions"
            >
            </highcharts-chart>
            <!-- <button class="btn" (click)="generateNewData()">
              Randomize Data
            </button> -->
          </div>
        </div>
      </div>
    </div>
  `,
})
export class HomeComponent {
  Highcharts: typeof Highcharts = Highcharts;
  updateFlag = false;

  data: Array<Highcharts.SeriesColumnOptions> = [
    {
      name: 'Income - Michael', // Category Name
      data: [148, 133, 124, 99], // Data array
      stack: 'Income', // Category Group Enum
      type: 'column',
    },
    {
      name: 'Income - Stephanie',
      data: [102, 98, 65, 50],
      stack: 'Income',
      type: 'column',
    },
    {
      name: 'Income - Investing',
      data: [113, 122, 95, 60],
      stack: 'Income',
      type: 'column',
    },
    {
      name: 'Income - Other',
      data: [113, 122, 95, 60],
      stack: 'Income',
      type: 'column',
    },
    {
      name: 'Spending - Needs',
      data: [77, 72, 80, 75],
      stack: 'Spending',
      type: 'column',
    },
    {
      name: 'Spending - Wants',
      data: [77, 72, 80, 75],
      stack: 'Spending',
      type: 'column',
    },
    {
      name: 'Spending - Passive',
      data: [77, 72, 80, 75],
      stack: 'Spending',
      type: 'column',
    },
    {
      name: 'Savings',
      data: [77, 72, 80, 75],
      stack: 'Saving',
      type: 'column',
    },
  ];

  chartOptions: Highcharts.Options = {
    chart: {
      type: 'column',
    },

    title: {
      text: 'Income Statement Grouped by Time Period',
      align: 'left',
    },

    xAxis: {
      categories: [
        'Previous Month',
        'Past 3 Months',
        'Past 6 Months',
        'Past Year',
      ],
    },

    yAxis: {
      allowDecimals: false,
      min: 0,
      title: {
        text: 'Currency (USD)',
      },
    },

    tooltip: {
      format:
        '<b>{key}</b><br/>{series.name}: {y}<br/>' +
        'Total: {point.stackTotal}',
    },

    legend: {
      layout: 'vertical',
      align: 'right',
      verticalAlign: 'top',
    },

    plotOptions: {
      column: {
        stacking: 'normal', //"percent",  // TODO: Can we do a % based?
      },
    },

    series: this.data,

    responsive: {
      rules: [
        {
          condition: {
            maxWidth: 500,
          },
          chartOptions: {
            legend: {
              layout: 'horizontal',
              align: 'center',
              verticalAlign: 'bottom',
            },
          },
        },
      ],
    },
  };

  lineChartOptions: Highcharts.Options = {
    series: [
      {
        data: [1, 2, 3],
        type: 'line',
      },
    ],
  };

  public generateNewData() {
    let newSeries: Array<Highcharts.SeriesColumnOptions> = [];
    (this.chartOptions.series as Array<Highcharts.SeriesColumnOptions>).forEach(
      (dataset) => {
        console.log('generating new data for: ', dataset.name);

        const newData = dataset.data
          ? dataset.data.map((data) => Math.round(Math.random() * 100))
          : [];

        newSeries.push({
          name: dataset.name,
          data: newData,
          stack: dataset.stack,
          type: 'column',
        });
      }
    );

    this.chartOptions.series = newSeries;
    this.chartOptions = { ...this.chartOptions };
  }
}
