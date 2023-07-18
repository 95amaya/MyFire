<script lang="ts">
  // console.log("App: starting...");
  import Card from "./components/Card.svelte";
  import Counter from "./components/Counter.svelte";
  import Chart from "./components/Chart.svelte";
  import container from "./inversify.config";
  import TYPES from "./types";
  import type { interfaces } from "inversify";

  // Test Prop Passing
  export let test: string;
  console.log(test);

  // inject chart directive factory into app to instantiate these
  // GOOD
  export let chartDirectiveFactory: interfaces.Factory<IChartDirective>;
  const chartDirective1 = chartDirectiveFactory() as IChartDirective;
  const chartDirective2 = chartDirectiveFactory() as IChartDirective;

  // BAD
  // const chartDirective1 = container.get<IChartDirective>(TYPES.IChartDirective);
  // const chartDirective2 = container.get<IChartDirective>(TYPES.IChartDirective);
  // console.log("App: finished loading.");
</script>

<main>
  <div class="bg-gray-500 flex flex-col h-screen">
    <div class="flex-1 w-full text-lg h-full shadow-lg bg-gray-300">
      <Card>
        <span slot="card-title">Income Statement</span>
        <!-- <Counter slot="card-body" /> -->
        <Chart chartDirective={chartDirective1} slot="card-body" />
      </Card>
      <Card>
        <span slot="card-title">Income Statement 2</span>
        <!-- <Counter slot="card-body" /> -->
        <Chart chartDirective={chartDirective2} slot="card-body" />
      </Card>
    </div>
  </div>
</main>
