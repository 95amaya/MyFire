import { injectable, inject } from "inversify";
import "reflect-metadata";
import TYPES from "../types";

@injectable()
class ChildTest implements IChildTest {
  public constructor() {}
  public getHelloWorld(): string {
    return "Hello World DI Test!";
  }
}
@injectable()
class ParentTest implements IParentTest {
  private _childTest: IChildTest;

  public constructor(@inject(TYPES.IChildTest) ChildTest: IChildTest) {
    this._childTest = ChildTest;
  }
  public sayHello(): string {
    return this._childTest.getHelloWorld();
  }
}

export { ChildTest, ParentTest };
