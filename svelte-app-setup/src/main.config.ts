import type { interfaces } from "inversify";
import container from "./inversify.config";
import TYPES from "./types";

let chartDirectiveFactory = container.get<interfaces.Factory<IChartDirective>>(
  TYPES.IFactoryOfIChartDirective
);

export { chartDirectiveFactory };

// Hello World Text Example
// const parentTest = container.get<IParentTest>(TYPES.IParentTest);
// console.log(parentTest.sayHello());

// Good Factory Example
// import { Container, inject, injectable, type interfaces } from "inversify";

// let TYPES = {
//   IWarrior: Symbol("IWarrior"),
//   IWeapon: Symbol("IWeapon"),
//   IFactoryOfIWarrior: Symbol("IFactory<IWarrior>"),
// };

// interface IWeapon {}

// @injectable()
// class Katana implements IWeapon {}

// interface IWarrior {
//   weapon: IWeapon;
//   rank: string;
// }

// @injectable()
// class Warrior implements IWarrior {
//   public weapon: IWeapon;
//   public rank: string;
//   public constructor(@inject(TYPES.IWeapon) weapon: IWeapon) {
//     this.weapon = weapon;
//     this.rank = null; // important!
//   }
// }

// let kernel = new Container();
// kernel.bind<IWarrior>(TYPES.IWarrior).to(Warrior);
// kernel.bind<IWeapon>(TYPES.IWeapon).to(Katana);

// kernel
//   .bind<interfaces.Factory<IWarrior>>(TYPES.IFactoryOfIWarrior)
//   .toFactory<IWarrior>((context) => {
//     return (rank: string) => {
//       console.log("running Factory");
//       let warrior = context.container.get<IWarrior>(TYPES.IWarrior);
//       warrior.rank = rank;
//       return warrior;
//     };
//   });

// let warriorFactory = kernel.get<interfaces.Factory<IWarrior>>(
//   TYPES.IFactoryOfIWarrior
// );

// let master = warriorFactory("master");
// let student = warriorFactory("student");
