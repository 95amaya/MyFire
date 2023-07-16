import { writable } from "svelte/store";

function createCount() {
  const { subscribe, set, update } = writable<number>(0);

  return {
    subscribe,
    increment: () => update((n: number) => n + 1),
    decrement: () => update((n: number) => n - 1),
    reset: () => set(0),
  };
}

export const count = createCount();
