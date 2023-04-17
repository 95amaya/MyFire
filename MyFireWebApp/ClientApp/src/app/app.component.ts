import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <body>
      <app-nav-menu></app-nav-menu>
      <main>
        <router-outlet></router-outlet>
      </main>
      <footer class="footer footer-center p-4 bg-base-300 text-base-content">
        <div>
          <p>Copyright © 2023 - All right reserved by ACME Industries Ltd</p>
        </div>
      </footer>
    </body>
  `,
})
export class AppComponent {
  title = 'app';
}
