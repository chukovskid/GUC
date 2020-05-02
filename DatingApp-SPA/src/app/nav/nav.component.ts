import { Component, OnInit } from '@angular/core';
import {AuthService} from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = { }; // ova ke gi cuva username i pass

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  login(){
    this.authService.login(this.model).subscribe(next => { // ovde ne ni mora da se prodolzi so next  na subscribe()
      console.log('Login e uspesno');
       }, error => {
         console.log(error);
       });
  }

logedin(){
  const token = localStorage.getItem('token'); // idi u local storage i zemi item so ime token (ova preku inspect>app>localStorage)
  return !!token; // ova vrakja true ili falce vo zavistnost dali postoi token
}

logeout(){
localStorage.removeItem('token');
console.log('logged out');
}


}
