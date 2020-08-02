import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  // potrebno e da gi zemam Roles od Data vo Routes
  canActivate(next: ActivatedRouteSnapshot): boolean {
    // ako nekoj go aktivira Admin,
    const roles = next.firstChild.data['roles'] as Array<string>; // roles ke bide populirano so vrednostite od Data vo Routes

    if (roles) {
      const match = this.authService.roleMatch(roles);
      if (match) {
        return true;
      } else {
        this.router.navigate(['members']);
        this.alertify.error('You are not authorised to acces this area');
      }
    }

    //
    if (this.authService.logedin()) {
      return true;
    }

    this.alertify.error('You shall not pass!!');
    this.router.navigate(['/home']);
  }
}
