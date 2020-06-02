import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {BehaviorSubject } from 'rxjs';
import {map} from 'rxjs/operators';
import {JwtHelperService} from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment'; // 84
import { User } from '../_models/user';
import { from } from 'rxjs';

@Injectable({
  providedIn: 'root' // root kaj nas e app.module.ts
}) // ccelovo @injectable se dodava bidejki services nemaat inject kako i site drugi modeli ( inportot e ov mislam)
export class AuthService {
  baseUrl = environment.apiUrl + 'auth/'; // 83
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>('../../assets/user.png'); // type of observable, that we subscribe to like  HTTP requests in Angular 118
  currentPhotoUrl = this.photoUrl;


constructor(private http: HttpClient) { }

changeMemberPhoto(photoUrl: string){ // ovaa funkcija ke ja vikam za da ja smeni slikata 118
   this.photoUrl.next(photoUrl);
}

login(model: any){
return this.http.post(this.baseUrl + 'login', model) // ova ke vrati TOKEN
.pipe(
  map((response: any) => {
      const user = response; // ova e TOKENOT koga ke se logira (dolgiot so mn cudni bukvi)
      if (user){
        localStorage.setItem('token', user.token); // go zacuvuvame tokenot LOKALNO
        localStorage.setItem('user', JSON.stringify(user.user)); // userot go dobivam u top od Object a getItem(prima strings samo)
        this.decodedToken = this.jwtHelper.decodeToken(user.token); // ova go dava usernameot
        this.currentUser = user.user; // ova go davam ponatamu za Spa da go koristi
        this.changeMemberPhoto(this.currentUser.photoUrl); // ja vikam ovaa funk za da ja smeni main photo // 118
        console.log(this.decodedToken);
      }
    }
  )
);
}
logedin(){
  const token = localStorage.getItem('token'); // zemi go token od localStorage
  return !this.jwtHelper.isTokenExpired(token); // provedi dali NE e expired
}


register(user: User){ // so ovoj metod prakjam model vo POST na linkot register i si se pravi nov user, voala
  return this.http.post(this.baseUrl + 'register', user); // bidejki ne vrakjame nisto so register mn e pokratok od LOGIN
}

}
