// Inversion of Control (IoC) container setup
console.log("Startup: starting...");

import { Container } from "inversify";
import { TYPES } from "./models/types";
import * as Interfaces from "./models/interfaces";
import { ChildTest, ParentTest } from "./models/test";

const myContainer = new Container();
myContainer.bind<Interfaces.IChildTest>(TYPES.IChildTest).to(ChildTest);
myContainer.bind<Interfaces.IParentTest>(TYPES.IParentTest).to(ParentTest);

export { myContainer };

console.log("Startup: Finished.");
