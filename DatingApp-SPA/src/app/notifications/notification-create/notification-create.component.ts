import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-notification-create',
  templateUrl: './notification-create.component.html',
  styleUrls: ['./notification-create.component.css'],
})
export class NotificationCreateComponent implements OnInit {
  notification: Notification;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>; // vaka mozam da sostapam do site Configs, opcii na primer max date i color

  constructor(
    private authService: AuthService,
    private userService: UserService,
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

  // id: number;
  // title: string;
  // description: string;
  // created: Date;
  // content: string;
  // readBy: User[];
  // readByCount: number;
  // notificationUser: NotificationUser[];

  createRegisterForm() {
    this.registerForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      content: ['', Validators.required],
    });
  }

  register() {
    // treba da gi zemam site vrednosti od spavo i da gi stavam vo api user
    if (this.registerForm.valid) {
      //  ako e VALID // 132
      this.notification = Object.assign({}, this.registerForm.value);

      this.userService
        .CreateNotification(
          this.authService.decodedToken.nameid,
          this.notification
        )
        .subscribe(
          () => {
            this.aletify.success('uspesno registrirano izvestuvanje');
          },
          (error) => {
            this.aletify.error('The Register failed');
          }, () => {
              this.router.navigate(['/notifications']);
            }
        ); // Sto ke prai na On COMPLITE koga ke uspee registracijata
    }
  }
  // cancel() {
  //   // se pali koga ke stisnam cancel na register formata
  //   this.cancelRegister.emit(false); // ke vrat vrednost FALSE
  //   this.aletify.message('canceld');
  // }
}
