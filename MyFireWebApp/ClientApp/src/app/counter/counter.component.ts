import { Component } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  template: `
    <div class="h4">Component Libray Examples</div>
    <div class="grid grid-cols-4">
      <div class="p-3 flex flex-column justify-around">
        <div class="h5">Button</div>
        <button class="btn btn-primary" (click)="incrementCounter()">
          Increment
        </button>
        {{ currentCount }}
      </div>
      <div class="p-3 flex flex-column justify-around">
        <div class="h5">Date</div>
        <input type="date" [(ngModel)]="myDate" />
        <div [ngClass]="{ show: !!myDate }">
          {{ myDate }}
        </div>
      </div>
      <div class="p-3 flex flex-column justify-around">
        <div class="h5">Dropdown</div>
        <ng-select
          [items]="carOptions"
          bindLabel="name"
          bindValue="id"
          [(ngModel)]="selectedCar"
        >
        </ng-select>
        <div *ngIf="selectedCar; else emptySelectedOption">
          {{ selectedCar }}
        </div>
        <ng-template #emptySelectedOption> No Option Selected </ng-template>
      </div>
    </div>
  `,
})
export class CounterComponent {
  public currentCount = 0;
  public myDate = null;
  public selectedCar = null;

  public carOptions = [
    { id: 1, name: 'Volvo' },
    { id: 2, name: 'Saab' },
    { id: 3, name: 'Opel' },
    { id: 4, name: 'Audi' },
  ];

  public incrementCounter() {
    this.currentCount++;
  }
}
