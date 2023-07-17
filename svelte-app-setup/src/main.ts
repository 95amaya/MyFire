console.log("Main: Starting...");
import "./app.css";
import "./startup";
// start
import { myContainer } from "./startup";
import { TYPES } from "./types";

const parentTest = myContainer.get<IParentTest>(TYPES.IParentTest);
console.log(parentTest.sayHello());

// done
import App from "./App.svelte";

const app = new App({
  target: document.getElementById("app"),
});
console.log("Main: Finished Loading App...");

export default app;
