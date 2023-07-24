<script lang="ts">
  // console.log("App: starting...");
  import type { inversify } from "./types";

  import Card from "./components/Card.svelte";
  import Counter from "./components/Counter.svelte";
  import Chart from "./components/Chart.svelte";

  // inject chart directive factory into app to instantiate these
  export let chartDirectiveFactory: inversify.SimpleFactory<
    IChartDirective,
    null[]
  >;

  export let chartConfigBuilder: IChartConfigBuilder;

  let incomeChartProps = <IChartProps>{
    chartDirective: chartDirectiveFactory(),
    chartConfigBuilder: chartConfigBuilder,
  };
</script>

<main>
  <div class="bg-gray-500 flex flex-col h-screen">
    <div class="flex-1 w-full text-lg h-full shadow-lg bg-gray-300">
      <Card>
        <span slot="card-title">Income Statement</span>
        <!-- <Counter slot="card-body" /> -->
        <Chart {...incomeChartProps} slot="card-body" />
      </Card>
      <!-- <Card>
        <span slot="card-title">Income Statement 2</span>
        <Chart chartDirective={spendingChartDirective} slot="card-body" />
      </Card> -->
    </div>
  </div>
</main>
