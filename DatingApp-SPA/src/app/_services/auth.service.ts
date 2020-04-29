import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';

@Injectable({
  providedIn: 'root' // root kaj nas e app.module.ts
}) // ccelovo @injectable se dodava bidejki services nemaat inject kako i site drugi modeli ( inportot e ov mislam)
export class AuthService {
  baseUrl = 'http://localhost:5000/api/auth/';

constructor(private http: HttpClient) { }

login(model: any){
return this.http.post(this.baseUrl + 'login', model) // ova ke vrati TOKEN
.pipe(
  map((response: any) => {
      const user = response; // ova e TOKENOT koga ke se logira (dolgiot so mn cudni bukvi)
      if ( user){
        localStorage.setItem('token', user.token); // go zacuvuvame tokenot LOKALNO
      }
    }
  )
);
}

register(model: any){ // so ovoj metod prakjam model vo POST na linkot register i si se pravi nov user, voala
  return this.http.post(this.baseUrl + 'register', model); // bidejki ne vrakjame nisto so register mn e pokratok od LOGIN
}

}
