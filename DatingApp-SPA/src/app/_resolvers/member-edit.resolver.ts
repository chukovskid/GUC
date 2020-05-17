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
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberEditResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private router: Router,  /// ne stavas ActivatedRouter
    private alertify: AlertifyService,
    private authService: AuthService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    // console.log(this.userService.getUser(this.authService.decodedToken.nameid));
    return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
        // .pipe i se natamu e za errors ..   // getUser ne treba subscribe poso ima samoto
        catchError((error) => {
          this.alertify.error('Problem in retriving your data');
          this.router.navigate(['/members']);
          return of(null);
        })
    );
  }
}
