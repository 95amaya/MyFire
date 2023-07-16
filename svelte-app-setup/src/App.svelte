<script lang="ts">
  console.log("App: starting...");
  import svelteLogo from "./assets/svelte.svg";
  import viteLogo from "/vite.svg";
  import Counter from "./lib/Counter.svelte";
  console.log("App: finished loading.");
</script>

<main>
  <h1 class="text-3xl font-bold underline">Hello world From Tailwind!</h1>
</main>

<style lang="postcss">
  :global(html) {
    background-color: theme(colors.gray.100);
  }
</style>
