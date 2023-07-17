// refer to https://medium.com/jspoint/typescript-type-declaration-files-4b29077c43 for more info

// interfaces.d.ts
// ts reference https://www.typescriptlang.org/docs/handbook/declaration-files/by-example.html
// TODO: Figure out how to declare test and chart namespaces that can be used globally

interface IChildTest {
  getHelloWorld(): string;
}

interface IParentTest {
  sayHello(): string;
}

interface IChart {
  update(
    options: object,
    redraw?: boolean,
    oneToOne?: boolean,
    animation?: boolean | object
  ): void;
  destroy(): void;
}

interface IChartFactory {
  Build: (
    renderTo: string | HTMLElement,
    options: object,
    callback?: (chart: IChart) => void
  ) => IChart;
}

interface IChartConfigFactory {
  Build: () => void;
}

interface IChartDirective {
  Render: (
    node: string | HTMLElement,
    config: object
  ) => Svelte.ActionReturn<object>;
}

// functions.d.ts
// ts reference https://www.typescriptlang.org/docs/handbook/declaration-files/by-example.html
// ts reference https://www.typescriptlang.org/docs/handbook/2/functions.html

type RenderChartDirective = (
  node: string | Highcharts.HTMLDOMElement,
  config: Highcharts.Options
) => Svelte.ActionReturn;
