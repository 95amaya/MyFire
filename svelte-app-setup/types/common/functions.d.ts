// ts reference https://www.typescriptlang.org/docs/handbook/declaration-files/by-example.html
// ts reference https://www.typescriptlang.org/docs/handbook/2/functions.html

type RenderChartDirective = (
  node: string | Highcharts.HTMLDOMElement,
  config: Highcharts.Options
) => Svelte.ActionReturn;
