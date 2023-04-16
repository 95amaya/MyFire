import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  styleUrls: ['./nav-menu.component.css'],
  template: `
    <header>
      <nav
        class="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3"
      >
        <div class="container">
          <a class="navbar-brand" [routerLink]="['/']">MyFireWebApp</a>
          <button
            class="navbar-toggler"
            type="button"
            data-toggle="collapse"
            data-target=".navbar-collapse"
            aria-label="Toggle navigation"
            [attr.aria-expanded]="isExpanded"
            (click)="toggle()"
          >
            <span class="navbar-toggler-icon"></span>
          </button>
          <div
            class="navbar-collapse collapse d-sm-inline-flex justify-content-end"
            [ngClass]="{ show: isExpanded }"
          >
            <ul class="navbar-nav flex-grow">
              <li
                class="nav-item"
                [routerLinkActive]="['link-active']"
                [routerLinkActiveOptions]="{ exact: true }"
              >
                <a class="nav-link text-dark" [routerLink]="['/']">Home</a>
              </li>
              <li class="nav-item" [routerLinkActive]="['link-active']">
                <a class="nav-link text-dark" [routerLink]="['/counter']"
                  >Counter</a
                >
              </li>
              <li class="nav-item" [routerLinkActive]="['link-active']">
                <a class="nav-link text-dark" [routerLink]="['/fetch-data']"
                  >Fetch data</a
                >
              </li>
            </ul>
          </div>
        </div>
      </nav>
    </header>
  `,
})
export class NavMenuComponent {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
