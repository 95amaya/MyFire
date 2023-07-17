import { injectable, inject } from "inversify";
import "reflect-metadata";
import { TYPES } from "../../types";
import * as Highcharts from "highcharts";
import type { ActionReturn } from "svelte/action";

@injectable()
class HighchartDirective implements IChartDirective {
  private _chartFactory: IChartFactory;

  public constructor(
    @inject(TYPES.IChartFactory) private highchartFactory: IChartFactory
  ) {
    this._chartFactory = highchartFactory;
  }

  Render = (
    node: string | HTMLElement,
    config: Highcharts.Options
  ): ActionReturn<Highcharts.Options> => {
    console.log("render...", node, config);
    const redraw = true;
    const oneToOne = true;
    const chart = this._chartFactory.Build(node, config);

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

export { HighchartDirective };
