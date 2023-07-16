<script lang="ts">
  import { count } from "../services/store";
</script>

<div class="container">
  <div class="container mx-auto">
    <p>count is {$count}</p>
  </div>
  <div class="container">
    <div class="grid grid-cols-3">
      <button class="btn" on:click={count.increment}>+</button>
      <button class="btn" on:click={count.decrement}>-</button>
      <button class="btn" on:click={count.reset}>reset</button>
    </div>
  </div>
</div>
