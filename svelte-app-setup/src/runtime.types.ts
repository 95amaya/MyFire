// Define type identifiers for runtime dependency mapping

const TYPES = {
  IChartBuilder: Symbol.for("IChartBuilder"),
  IChartDirective: Symbol.for("IChartDirective"),
  IFactoryOfIChartDirective: Symbol("IFactory<IChartDirective>"),
  IChartConfigBuilder: Symbol("IChartConfigBuilder"),
};

export default TYPES;
