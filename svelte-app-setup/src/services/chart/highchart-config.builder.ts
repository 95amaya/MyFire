import { injectable, inject } from "inversify";
import "reflect-metadata";
import type { highcharts } from "../../types";

@injectable()
export class HighchartConfigBuilder implements IChartConfigBuilder {
  BuildRollingSummaryConfig = (
    series: Array<highcharts.SeriesColumnOptions>
  ): highcharts.Options => {
    return {
      chart: {
        type: "column",
      },

      title: {
        text: "Income Statement Grouped by Time Period",
        align: "left",
      },

      xAxis: {
        categories: [
          "Previous Month",
          "Past 3 Months",
          "Past 6 Months",
          "Past Year",
        ],
      },

      yAxis: {
        allowDecimals: false,
        min: 0,
        title: {
          text: "Currency (USD)",
        },
      },

      tooltip: {
        format:
          "<b>{key}</b><br/>{series.name}: {y}<br/>" +
          "Total: {point.stackTotal}",
      },

      legend: {
        layout: "vertical",
        align: "right",
        verticalAlign: "top",
      },

      plotOptions: {
        column: {
          stacking: "percent", //"normal",  // TODO: Can we do a % based?
        },
      },

      series: series,

      responsive: {
        rules: [
          {
            condition: {
              maxWidth: 500,
            },
            chartOptions: {
              legend: {
                layout: "horizontal",
                align: "center",
                verticalAlign: "bottom",
              },
            },
          },
        ],
      },
    };
  };

  BuildTrendChartConfig = (series: object[]): object => {
    return {};
  };
}
