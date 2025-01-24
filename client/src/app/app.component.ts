import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  http = inject(HttpClient); // dependency injection w nowym Angularze zamiast konstruktora
  title = 'DatingApp';
  users: any;

  ngOnInit(): void { // aby użyć parametrów klasy musimy dać przypis this.
    this.http.get('https://localhost:5001/api/users').subscribe({ // definiujemy pod jakim url wykona się funkcja (zgodna z API)
      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log('Request has completed')
    }) 
  }
}
