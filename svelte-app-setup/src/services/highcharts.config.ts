import * as Highcharts from "highcharts";

export default (
  node: string | Highcharts.HTMLDOMElement,
  config: Highcharts.Options
) => {
  const redraw = true;
  const oneToOne = true;
  const chart = Highcharts.chart(node, config);

  return {
    update(config: Highcharts.Options) {
      chart.update(config, redraw, oneToOne);
    },
    destroy() {
      chart.destroy();
    },
  };
};
