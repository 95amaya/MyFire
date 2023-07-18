// Define type identifiers for runtime dependency mapping

const TYPES = {
  IChildTest: Symbol.for("IChildTest"),
  IParentTest: Symbol.for("IParentTest"),
  IChartFactory: Symbol.for("IChartFactory"),
  IChartDirective: Symbol.for("IChartDirective"),
  IFactoryOfIChartDirective: Symbol("IFactory<IChartDirective>"),
};

export default TYPES;
