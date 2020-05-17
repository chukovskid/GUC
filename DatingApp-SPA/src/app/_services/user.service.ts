import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';




@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;

constructor(private http: HttpClient) { }

getUsers(): Observable<User[]> { // getUsers() ke ni vrati Observable<>
  return this.http.get<User[]>(this.baseUrl + 'users'); // zemi User od toj Url // vo 89 trgam httpOptions bidejki staviv Jwt
}
getUser(id): Observable<User> {
  return this.http.get<User>(this.baseUrl + 'users/' + id);
}
updateUser(id: number, user: User){
  return this.http.put(this.baseUrl + 'users/' + id, user); // dava id i user ?
}


}
