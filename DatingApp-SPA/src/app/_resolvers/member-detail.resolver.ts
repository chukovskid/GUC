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
export class MemberDetailResolver implements Resolve<User> {
  constructor(
    private userService: UserService,
    private router: Router,  /// ne stavas ActivatedRouter
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    return this.userService.getUser(route.params['id']) // do tuka e samo da go  najde USEROT so toa ID .. metodov ide u edit-details (pred da pomine niz routes.ts)
    .pipe(   // .pipe i se natamu e za errors ..   // getUser ne treba subscribe poso ima samoto
        catchError((error) => {
          this.alertify.error('Problem in retriving data');
          this.router.navigate(['/members']);
          return of(null);
        })
    );
  }
}
