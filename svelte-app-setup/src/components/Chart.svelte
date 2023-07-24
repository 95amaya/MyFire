<script lang="ts">
  import { onMount } from "svelte";
  import { type highcharts } from "../types";

  export let chartDirective: IChartDirective;
  export let chartConfigBuilder: IChartConfigBuilder;
  let config: highcharts.Options = {};

  onMount(async () => {
    const series = await new Promise<object[]>((resolve, reject) => {
      let resp = [
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
      ];

      resolve(resp);
    });

    config = chartConfigBuilder.BuildRollingSummaryConfig(series);
  });

  function generateNewData() {
    let newSeries = [];
    config.series.forEach((dataset: highcharts.SeriesColumnOptions) => {
      console.log("generating new data for: ", dataset.name);

      const newData = dataset.data.map((data) =>
        Math.round(Math.random() * 100)
      );
      newSeries.push({
        name: dataset.name,
        data: newData,
        stack: dataset.stack,
      });
    });
    config = chartConfigBuilder.BuildRollingSummaryConfig(newSeries);
  }
</script>

<div class="chart" use:chartDirective.Render={config} />

<button class="btn" on:click={generateNewData}> Randomize Data </button>
