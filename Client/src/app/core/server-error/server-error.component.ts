import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss']
})
export class ServerErrorComponent implements OnInit {
  error: any;

  constructor(private router: Router) {
    const navivation = this.router.getCurrentNavigation();
    this.error = navivation && navivation.extras && navivation.extras.state && navivation.extras.state.error;
  }

  ngOnInit(): void {
  }

}
