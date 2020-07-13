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
import { Message } from '../_models/message';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MessagesResolver implements Resolve<Message[]> { // 93
pageNumber = 1;
pageSize = 5;
messageContainer = 'Unread';

  constructor(
    private userService: UserService,
    private router: Router,  /// ne stavas ActivatedRouter
    private alertify: AlertifyService,
    private authService: AuthService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
    return this.userService.getMessages(this.authService.decodedToken.nameid, this.pageNumber, this.pageSize, this.messageContainer).pipe(
        // .pipe i se natamu e za errors ..   // getUser ne treba subscribe poso ima samoto
        catchError((error) => {
          this.alertify.error('Problem in retriving messages');
          this.router.navigate(['/home']);
          return of(null);
        })
    );
  }
}
