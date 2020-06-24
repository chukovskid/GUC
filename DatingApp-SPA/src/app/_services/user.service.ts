import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { isNullOrUndefined } from 'util';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  // GET USERS
  getUsers(page?, itemsPerPage?, userParams?, likesParams?): Observable<PaginatedResult<User[]>> {
    // smeneto 143, Pagination! // likeParams 157
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>(); // nov paginatetResult (useri, userParams)
    let params = new HttpParams(); // 143

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page); // append menuva vrednost vo BODY, pageNumber = page sega
      params = params.append('pageSize', itemsPerPage);
    }
    if (userParams != null)
    {
      params = params.append('maxAge', userParams.maxAge); // informaciite zemeni od front, zameni gi so vrednosta od API
      params = params.append('minAge', userParams.minAge);
      params = params.append('gender', userParams.gender);
      params = params.append('orderBy', userParams.orderBy);
    }

    // Likes List
    if (likesParams === 'Likers'){
      params = params.append('Likers', 'true');
    }
    if (likesParams === 'Likees'){
      params = params.append('Likees', 'true');
    }

    return this.http
      .get<User[]>(this.baseUrl + 'users', { observe: 'response', params}) // zemi User od toj Url // vo 89 trgam httpOptions bidejki staviv Jwt
      .pipe(
        map((response) => {
          paginatedResult.result = response.body; // vrakjam Useri, toa se cuva vo result
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            ); // ova se UserParams
          }
          return paginatedResult;
        })
      );
  }
  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }
  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user); // dava id i user ?
  }
  setMainPhoto(userId: number, id: number) {
    return this.http.post(
      this.baseUrl + 'users/' + userId + '/photos/' + id + '/setMain',
      {}
    ); // pri klikanje funkcijava nosi na 5000 url koj ja pravi slikava isMain
    // prativ prazen objekt kolku da go zadovolam uslovot na POST
  }
  deletePhoto(userId: number, id: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/photos/' + id);
  }
  sendLike(id: number, recipientId: number){
    return this.http.post(this.baseUrl + 'users/' + id + '/like/' + recipientId, {});
  }
}
