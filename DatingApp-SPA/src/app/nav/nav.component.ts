import { Component, OnInit } from '@angular/core';
import {AuthService} from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = { }; // ova ke gi cuva username i pass
  photoUrl: string; // nova promenliva kade ke cuvam photo od mainPhotoUlr

  constructor(public authService: AuthService, private alrtify: AlertifyService, private router: Router) { }

  ngOnInit() {
    this.authService.currentPhotoUrl.subscribe(photoUrl => this.photoUrl = photoUrl); // od auth ja zimam za da ja nosam vo Nav 118
  }

  login(){
    this.authService.login(this.model).subscribe(next => { // ovde ne ni mora da se prodolzi so next  na subscribe()
      this.alrtify.success('Logged in successfuly');
    }, error => {
      this.alrtify.success(error);
    }, () => {
      this.router.navigate(['/members']);
    });
  }

logedin(){
  return this.authService.logedin();
}




logeout(){
localStorage.removeItem('token');
localStorage.removeItem('user');
this.authService.decodedToken = null;
this.authService.currentUser = null;
this.alrtify.message('Logged out');
this.router.navigate(['/home']);

}


}
