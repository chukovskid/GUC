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
import { Room } from '../_models/room';

@Injectable()
export class NotificationResolver implements Resolve<Notification> {
  constructor(
    private userService: UserService,
    private router: Router, /// ne stavas ActivatedRouter
    private alertify: AlertifyService,
    private authService: AuthService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Notification> {
    return this.userService.getNotification(this.authService.decodedToken.nameid, route.params['id']).pipe(
      catchError((error) => {
        this.alertify.error('error od NotificationResolver');
        this.router.navigate(['/home']);
        return of(null);
      })
    );
  }
}
