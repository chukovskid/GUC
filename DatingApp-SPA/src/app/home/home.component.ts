import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;

  constructor(private http: HttpClient) { } // ova go zima linkot

  ngOnInit() {
  }

  registerToggle(){ // so ova VLAGAME vo REGISTER MODE (true)
    this.registerMode = true; // poso e toggle ke vrti pomegu true i false na register mode
  }




cancelRegisterMode(registerMode: boolean){
  this.registerMode = registerMode;
}

}
