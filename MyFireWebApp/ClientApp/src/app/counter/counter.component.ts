import { Component } from '@angular/core';

@Component({
  selector: 'app-counter-component',
  template: `
    <div class="text-xl">Component Libray Examples</div>
    <div class="grid grid-cols-4">
      <div class="p-3 flex flex-col justify-around">
        <div class="text-lg">Buttons</div>
        <button
          type="button"
          (click)="incrementCounter()"
          class="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
        >
          Default
        </button>
        <button
          type="button"
          (click)="incrementCounter()"
          class="py-2.5 px-5 mr-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-200 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700"
        >
          Alternative
        </button>
        <button
          type="button"
          (click)="incrementCounter()"
          class="text-white bg-gray-800 hover:bg-gray-900 focus:outline-none focus:ring-4 focus:ring-gray-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-gray-800 dark:hover:bg-gray-700 dark:focus:ring-gray-700 dark:border-gray-700"
        >
          Dark
        </button>
        <button
          type="button"
          (click)="incrementCounter()"
          class="text-gray-900 bg-white border border-gray-300 focus:outline-none hover:bg-gray-100 focus:ring-4 focus:ring-gray-200 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-gray-800 dark:text-white dark:border-gray-600 dark:hover:bg-gray-700 dark:hover:border-gray-600 dark:focus:ring-gray-700"
        >
          Light
        </button>
        <button
          type="button"
          (click)="incrementCounter()"
          class="focus:outline-none text-white bg-green-700 hover:bg-green-800 focus:ring-4 focus:ring-green-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-green-600 dark:hover:bg-green-700 dark:focus:ring-green-800"
        >
          Green
        </button>
        <button
          type="button"
          (click)="incrementCounter()"
          class="focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:bg-red-600 dark:hover:bg-red-700 dark:focus:ring-red-900"
        >
          Red
        </button>
        <button
          type="button"
          (click)="incrementCounter()"
          class="focus:outline-none text-white bg-yellow-400 hover:bg-yellow-500 focus:ring-4 focus:ring-yellow-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 dark:focus:ring-yellow-900"
        >
          Yellow
        </button>
        <button
          type="button"
          (click)="incrementCounter()"
          class="focus:outline-none text-white bg-purple-700 hover:bg-purple-800 focus:ring-4 focus:ring-purple-300 font-medium rounded-lg text-sm px-5 py-2.5 mb-2 dark:bg-purple-600 dark:hover:bg-purple-700 dark:focus:ring-purple-900"
        >
          Purple
        </button>
        {{ currentCount }}
      </div>
      <div class="p-3 flex flex-col justify-around">
        <div class="h5">Date</div>
        <input type="date" [(ngModel)]="myDate" />
        <div [ngClass]="{ show: !!myDate }">
          {{ myDate }}
        </div>
      </div>
      <div class="p-3 flex flex-col justify-around">
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
