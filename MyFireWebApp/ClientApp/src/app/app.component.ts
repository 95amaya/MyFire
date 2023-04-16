import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <body>
      <app-nav-menu></app-nav-menu>
      <main class="container mx-auto px-4">
        <router-outlet></router-outlet>
      </main>
    </body>
  `,
})
export class AppComponent {
  title = 'app';
}
