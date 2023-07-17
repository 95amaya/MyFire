// Inversion of Control (IoC) container setup
console.log("Startup: starting...");

import { Container } from "inversify";
import { TYPES } from "./types";
import { ChildTest, ParentTest } from "./services/test";
import { HighchartFactory } from "./services/chart/highchart.factory";
import { HighchartDirective } from "./services/chart/highchart.directive";

const myContainer = new Container();
myContainer.bind<IChildTest>(TYPES.IChildTest).to(ChildTest);
myContainer.bind<IParentTest>(TYPES.IParentTest).to(ParentTest);
myContainer.bind<IChartFactory>(TYPES.IChartFactory).to(HighchartFactory);
myContainer.bind<IChartDirective>(TYPES.IChartDirective).to(HighchartDirective);

export { myContainer };

console.log("Startup: Finished.");
