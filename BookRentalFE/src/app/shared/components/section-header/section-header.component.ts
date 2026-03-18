import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.css']
})
export class SectionHeaderComponent {

  constructor(public bcService: BreadcrumbService, public router: Router) { }

  ignorePath(): boolean {
    return this.router.url.includes('/admin') ||
      this.router.url.includes('/login') ||
      this.router.url.includes('/register')
  }

}
