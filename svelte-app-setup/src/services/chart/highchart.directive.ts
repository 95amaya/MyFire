import { injectable, inject } from "inversify";
import "reflect-metadata";
import TYPES from "../../runtime.types";
import type { ActionReturn } from "../../types";

@injectable()
export class HighchartDirective implements IChartDirective {
  private _chartBuilder: IChartBuilder;

  public constructor(
    @inject(TYPES.IChartBuilder) private highchartBuilder: IChartBuilder
  ) {
    console.log("HighchartDirective Instantiating...");
    this._chartBuilder = highchartBuilder;
  }

  public Render = (
    node: string | HTMLElement,
    config: Highcharts.Options
  ): ActionReturn<Highcharts.Options> => {
    // console.log("render...", node, config);
    const redraw = true;
    const oneToOne = true;
    const chart = this._chartBuilder.Build(node, config);

    return {
      update(config) {
        chart.update(config, redraw, oneToOne);
      },
      destroy() {
        chart.destroy();
      },
    };
  };
}
