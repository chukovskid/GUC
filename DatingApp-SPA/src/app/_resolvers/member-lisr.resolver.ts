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
export class MemberListResolver implements Resolve<User[]> { //93
  constructor(
    private userService: UserService,
    private router: Router,  /// ne stavas ActivatedRouter
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
    return this.userService.getUsers().pipe(
        // .pipe i se natamu e za errors ..   // getUser ne treba subscribe poso ima samoto
        catchError((error) => {
          this.alertify.error('Problem in retriving data');
          this.router.navigate(['/home']);
          return of(null);
        })
    );
  }
}
