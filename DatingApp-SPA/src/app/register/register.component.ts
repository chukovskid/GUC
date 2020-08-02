import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import {
  FormControl,
  FormGroup,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { User } from '../_models/user';
import { __assign } from 'tslib';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  user: User; // pazi na vakvi detali, objekt dobivas!
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>; // vaka mozam da sostapam do site Configs, opcii na primer max date i color

  constructor(
    private authService: AuthService,
    private aletify: AlertifyService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    (this.bsConfig = {
      containerClass: 'theme-dark-blue',
    }),
      this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        gender: ['male'],
        username: ['', Validators.required],
        knownAs: ['', Validators.required],
        dateOfBirth: [null, Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: [
          '',
          [
            Validators.required,
            Validators.maxLength(14),
            Validators.minLength(3),
          ],
        ], // 127
        confirmPassword: ['', Validators.required],
      },
      { validator: this.passwordMatchValidator }
    );
  }

  passwordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  register() {
    // treba da gi zemam site vrednosti od spavo i da gi stavam vo api user
    if (this.registerForm.valid) {
      //  ako e VALID // 132
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(
        () => {
          this.aletify.success('uspesno registriran');
        },
        (error) => {
          this.aletify.error('The Register failed');
        },
        () => {
          this.authService.login(this.user).subscribe(() => {
            this.router.navigate(['/members']);
          });
        }
      ); // Sto ke prai na On COMPLITE koga ke uspee registracijata
    }
  }
  cancel() {
    // se pali koga ke stisnam cancel na register formata
    this.cancelRegister.emit(false); // ke vrat vrednost FALSE
    this.aletify.message('canceld');
  }
}
