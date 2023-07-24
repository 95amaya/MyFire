// Inversion of Control (IoC) container setup
// console.log("Startup: configuring DI container...");

import type { inversify } from "./types";
import TYPES from "./runtime.types";
import { Container } from "inversify";
import { HighchartBuilder } from "./services/chart/highchart.builder";
import { HighchartDirective } from "./services/chart/highchart.directive";
import { HighchartConfigBuilder } from "./services/chart/highchart-config.builder";

const container = new Container();
container.bind<IChartBuilder>(TYPES.IChartBuilder).to(HighchartBuilder);
container.bind<IChartDirective>(TYPES.IChartDirective).to(HighchartDirective);
container
  .bind<inversify.SimpleFactory<IChartDirective, null[]>>(
    TYPES.IFactoryOfIChartDirective
  )
  .toFactory<IChartDirective, null[]>((context) => {
    return () => {
      console.log("running chart directive factory...");
      return context.container.get<IChartDirective>(TYPES.IChartDirective);
    };
  });

container
  .bind<IChartConfigBuilder>(TYPES.IChartConfigBuilder)
  .to(HighchartConfigBuilder);

export default container;

console.log("Startup: Finished configuring DI container...");
