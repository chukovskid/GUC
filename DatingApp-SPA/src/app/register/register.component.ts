import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {}; // pazi na vakvi detali, objekt dobivas!

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  register(){
    this.authService.register(this.model).subscribe(() => {
      console.log('registration succesful');
    }, error => {
      console.log(error);
    });
  }
  cancel(){ // se pali koga ke stisnam cancel na register formata
    this.cancelRegister.emit(false); // ke vrat vrednost FALSE
    console.log('canceld');
  }

}
