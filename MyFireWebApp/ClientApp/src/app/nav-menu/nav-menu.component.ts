import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  template: ` <nav
    class="bg-white border-gray-200 dark:bg-gray-900 shadow-sm mb-4"
  >
    <div
      class="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto px-4 py-2"
    >
      <a class="flex items-center h3" [routerLink]="['/']">MyFireWebApp</a>
      <button
        data-collapse-toggle="navbar-default"
        type="button"
        class="inline-flex items-center p-2 ml-3 text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600"
        aria-controls="navbar-default"
        aria-expanded="false"
        (click)="toggle()"
      >
        <svg
          class="w-6 h-6"
          aria-hidden="true"
          fill="currentColor"
          viewBox="0 0 20 20"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            fill-rule="evenodd"
            d="M3 5a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 10a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1zM3 15a1 1 0 011-1h12a1 1 0 110 2H4a1 1 0 01-1-1z"
            clip-rule="evenodd"
          ></path>
        </svg>
      </button>
      <div
        class="w-full md:block md:w-auto"
        id="navbar-default"
        [ngClass]="{ show: isExpanded, hidden: !isExpanded }"
      >
        <ul
          class="font-medium flex flex-col p-2 md:p-0 md:flex-row md:space-x-8 md:mt-0 md:border-0"
        >
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
  </nav>`,
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
