import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { MatCard } from '@angular/material/card';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  standalone: true,
  imports: [
    MatCard
  ],
  templateUrl: './server-error.component.html',
  styleUrl: './server-error.component.scss'
})
export class ServerErrorComponent {
  // look up ep. 98 for more information (second half) (why 'any' and not 'HttpErrorResponse'?)
  error?: any;

  // look up ep. 98 for more information
  constructor(private router: Router) {
    const navigation = this.router.getCurrentNavigation();
    this.error = navigation?.extras.state?.['error'];
  }
}
