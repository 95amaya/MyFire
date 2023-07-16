console.log("Main: Starting...");
import "./app.css";
import "./startup";
// start
import { myContainer } from "./startup";
import { TYPES } from "./models/types";
import * as Interfaces from "./models/interfaces";

const parentTest = myContainer.get<Interfaces.IParentTest>(TYPES.IParentTest);
console.log(parentTest.sayHello());

// done
import App from "./App.svelte";

const app = new App({
  target: document.getElementById("app"),
});
console.log("Main: Finished Loading App...");

export default app;
