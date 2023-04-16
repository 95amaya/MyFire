import { Component } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  template: `
    <h1>Counter</h1>

    <p>This is a simple example of an Angular component.</p>

    <p aria-live="polite">
      Current count: <strong>{{ currentCount }}</strong>
    </p>

    <button class="btn btn-primary" (click)="incrementCounter()">
      Increment
    </button>
  `,
})
export class CounterComponent {
  public currentCount = 0;

  public incrementCounter() {
    this.currentCount++;
  }
}
