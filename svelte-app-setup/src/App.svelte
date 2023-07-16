<script lang="ts">
  console.log("App: starting...");
  import Card from "./components/Card.svelte";
  import Counter from "./components/Counter.svelte";
  import Chart from "./components/Chart.svelte";
  console.log("App: finished loading.");
</script>

<main>
  <div class="bg-gray-500 flex flex-col h-screen">
    <div class="flex-1 w-full text-lg h-full shadow-lg bg-gray-300">
      <Card>
        <span slot="card-title">Income Statement</span>
        <!-- <Counter slot="card-body" /> -->
        <Chart slot="card-body" />
      </Card>
    </div>
  </div>
</main>
