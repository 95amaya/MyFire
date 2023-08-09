import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';

@Component({
  selector: 'app-counter-component',
  template: `
    <div class="hero min-h-screen bg-base-200">
      <div class="hero min-h-screen bg-base-200">
        <div class="hero-content text-center">
          <div class="max-w-md">
            <h1 class="text-5xl font-bold">Demos</h1>
            <p class="py-6">
              Provident cupiditate voluptatem et in. Quaerat fugiat ut assumenda
              excepturi exercitationem quasi. In deleniti eaque aut repudiandae
              et a id nisi.
            </p>
            <div class="form-control">
              <label class="label cursor-pointer">
                <span class="label-text">Toggle Dropdown Custom CSS</span>
                <input
                  type="checkbox"
                  class="toggle"
                  [(ngModel)]="isChecked"
                  (change)="ToggleCustomCss()"
                />
              </label>
            </div>
            <ng-select
              [style.--theme-height.rem]="themeHeight.value"
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
      </div>
    </div>
  `,
})
export class CounterComponent {
  public selectedCar = null;
  public themeHeight = new FormControl(4);
  public isChecked = false;

  public carOptions = [
    { id: 1, name: 'Volvo' },
    { id: 2, name: 'Saab' },
    { id: 3, name: 'Opel' },
    { id: 4, name: 'Audi' },
  ];

  public ToggleCustomCss() {
    if (this.isChecked) {
      this.themeHeight.setValue(6);
      // this.isChecked.setValue(false);
    } else {
      this.themeHeight.setValue(2);
      // this.isChecked.setValue(true);
    }
    console.log('Toggled', this.isChecked, this.themeHeight.value);
  }
}
