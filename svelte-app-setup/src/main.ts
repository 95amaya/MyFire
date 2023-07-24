// console.log("Main: Starting...");
import "./app.css";
import * as dependecies from "./main.config";
import App from "./App.svelte";

const app = new App({
  target: document.getElementById("app"),
  props: dependecies,
});
console.log("Main: Finished Loading App...");

export default app;
