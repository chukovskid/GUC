import { Injectable } from '@angular/core'; // 93
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import {
  Resolve,
  Router,
  ActivatedRoute,
  ActivatedRouteSnapshot,
} from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ListsResolver implements Resolve<User[]> { // 93
pageNumber = 1;
pageSize = 5; // novi vrednosti porzlicni od tie na API (=10)
likesParams = 'Likers'; // ousers who Liked our users (folowers)

  constructor(
    private userService: UserService,
    private router: Router,  /// ne stavas ActivatedRouter
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
    return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likesParams).pipe(
        // .pipe i se natamu e za errors ..   // getUser ne treba subscribe poso ima samoto
        catchError((error) => {
          this.alertify.error('Problem in retriving data');
          this.router.navigate(['/home']);
          return of(null);
        })
    );
  }
}
