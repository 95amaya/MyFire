import Highcharts from "highcharts";
import { injectable, inject } from "inversify";
import "reflect-metadata";

@injectable()
export class HighchartBuilder implements IChartBuilder {
  Build = (
    renderTo: string | HTMLElement,
    options: Highcharts.Options,
    callback?: Highcharts.ChartCallbackFunction
  ): Highcharts.Chart => Highcharts.chart(renderTo, options, callback);
}
