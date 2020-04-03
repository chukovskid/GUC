import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
values: any;

// ova e a API, da si gi zememe vrednostite
  constructor(private http: HttpClient) { } 
// mora ova za da koristime http
  ngOnInit() {
    this.getValues(); // da gi zeme vrednostite pri pustanje
  }

  getValues(){
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.values = response; // ova ke go ima objektot so vrednosti vo nego i ke gi stavi u VALUES

    }, error => {
      console.log(error);
    });
    // .get(SI BARA SUBSCRIBE bidejki e OBSERVABLE)
  }// gi zimame vrednostite od local host link

}
