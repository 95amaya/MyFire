// ts reference https://www.typescriptlang.org/docs/handbook/declaration-files/by-example.html

interface IChart {
  update(
    options: object,
    redraw?: boolean,
    oneToOne?: boolean,
    animation?: boolean | object
  ): void;
  destroy(): void;
}

interface IChartBuilder {
  Build: (
    renderTo: string | HTMLElement,
    options: object,
    callback?: (chart: IChart) => void
  ) => IChart;
}

interface IChartConfigBuilder {
  BuildRollingSummaryConfig: (series: object[]) => object;
  BuildTrendChartConfig: (series: object[]) => object;
}

interface IChartDirective {
  Render: (
    node: string | HTMLElement,
    config: object
  ) => Svelte.ActionReturn<object>;
}

interface IChartProps {
  chartDirective: IChartDirective;
  chartConfigBuilder: IChartConfigBuilder;
}
