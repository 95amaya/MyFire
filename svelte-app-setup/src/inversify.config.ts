// Inversion of Control (IoC) container setup
// console.log("Startup: configuring DI container...");

import { Container, type interfaces } from "inversify";
import TYPES from "./types";
import { ChildTest, ParentTest } from "./services/test";
import { HighchartFactory } from "./services/chart/highchart.factory";
import { HighchartDirective } from "./services/chart/highchart.directive";

const container = new Container();
container.bind<IChildTest>(TYPES.IChildTest).to(ChildTest);
container.bind<IParentTest>(TYPES.IParentTest).to(ParentTest);
container.bind<IChartFactory>(TYPES.IChartFactory).to(HighchartFactory);
container.bind<IChartDirective>(TYPES.IChartDirective).to(HighchartDirective);
container
  .bind<interfaces.Factory<IChartDirective>>(TYPES.IFactoryOfIChartDirective)
  .toFactory<IChartDirective>((context) => {
    return () => {
      console.log("running chart directive factory...");
      return context.container.get<IChartDirective>(TYPES.IChartDirective);
    };
  });

export default container;

console.log("Startup: Finished configuring DI container...");
