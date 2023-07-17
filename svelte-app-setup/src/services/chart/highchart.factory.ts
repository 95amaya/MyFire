import { injectable, inject } from "inversify";
import "reflect-metadata";
import * as Highcharts from "highcharts";

@injectable()
class HighchartFactory implements IChartFactory {
  Build = (
    renderTo: string | HTMLElement,
    options: Highcharts.Options,
    callback?: Highcharts.ChartCallbackFunction
  ): Highcharts.Chart => Highcharts.chart(renderTo, options, callback);
}

export { HighchartFactory };
