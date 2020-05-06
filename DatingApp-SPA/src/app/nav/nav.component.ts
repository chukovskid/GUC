import { Component, OnInit } from '@angular/core';
import {AuthService} from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = { }; // ova ke gi cuva username i pass

  constructor(public authService: AuthService, private alrtify: AlertifyService) { }

  ngOnInit() {
  }

  login(){
    this.authService.login(this.model).subscribe(next => { // ovde ne ni mora da se prodolzi so next  na subscribe()
      this.alrtify.success('Logged in successfuly');
    }, error => {
      this.alrtify.success(error);
       });
  }

logedin(){
  return this.authService.logedin();
}




logeout(){
localStorage.removeItem('token');
this.alrtify.message('Logged out');
}


}
