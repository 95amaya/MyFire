import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  template: ` <div class="navbar bg-base-100">
    <div class="flex-1">
      <img src="/assets/flame-icon.png" class="h-[50px]" />
      <h3 class="text-xl font-bold">F.I.R.E</h3>
    </div>
    <div class="flex-none">
      <ul class="menu menu-horizontal px-1">
        <li
          class="nav-item"
          [routerLinkActive]="['link-active']"
          [routerLinkActiveOptions]="{ exact: true }"
        >
          <a class="nav-link text-dark" [routerLink]="['/']">Home</a>
        </li>
        <li class="nav-item" [routerLinkActive]="['link-active']">
          <a class="nav-link text-dark" [routerLink]="['/counter']">Counter</a>
        </li>
        <li class="nav-item" [routerLinkActive]="['link-active']">
          <a class="nav-link text-dark" [routerLink]="['/fetch-data']"
            >Fetch data</a
          >
        </li>
      </ul>
    </div>
  </div>`,
})
export class NavMenuComponent {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    console.log(this.isExpanded);
    this.isExpanded = !this.isExpanded;
  }
}
