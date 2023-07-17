<script lang="ts">
  // import DrawChartDirective from "../services/chart.directive";
  import { myContainer } from "../startup";
  import * as HighCharts from "highcharts";
  import { TYPES } from "../types";
  const chartDirective = myContainer.get<IChartDirective>(
    TYPES.IChartDirective
  );

  console.log("Loading chart...");
  let config: HighCharts.Options = {
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

    series: [
      {
        name: "Income - Michael", // Category Name
        data: [148, 133, 124, 99], // Data array
        stack: "Income", // Category Group Enum
      },
      {
        name: "Income - Stephanie",
        data: [102, 98, 65, 50],
        stack: "Income",
      },
      {
        name: "Income - Investing",
        data: [113, 122, 95, 60],
        stack: "Income",
      },
      {
        name: "Income - Other",
        data: [113, 122, 95, 60],
        stack: "Income",
      },
      {
        name: "Spending - Needs",
        data: [77, 72, 80, 75],
        stack: "Spending",
      },
      {
        name: "Spending - Wants",
        data: [77, 72, 80, 75],
        stack: "Spending",
      },
      {
        name: "Spending - Passive",
        data: [77, 72, 80, 75],
        stack: "Spending",
      },
      {
        name: "Savings",
        data: [77, 72, 80, 75],
        stack: "Saving",
      },
    ] as Array<HighCharts.SeriesColumnOptions>,

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

  function generateNewData() {
    // let newSeries = [];
    // config.series.forEach((seri) => {
    //   console.log("generating new data for: ", seri.name);
    //   const newData = seri.data.map((data) => Math.round(Math.random() * 100));
    //   newSeries.push({
    //     name: seri.name,
    //     data: newData,
    //   });
    // });
    // config.series = newSeries;
  }
</script>

<!-- <div class="chart" use:DrawChartDirective={config} /> -->
<div class="chart" use:chartDirective.Render={config} />

<button class="btn" on:click={generateNewData}> Randomize Data </button>
