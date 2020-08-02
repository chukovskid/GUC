import { Component, OnInit, EventEmitter, Output } from '@angular/core';
import { Room } from 'src/app/_models/room';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-room-register',
  templateUrl: './room-register.component.html',
  styleUrls: ['./room-register.component.css'],
})
export class RoomRegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  room: Room;
  listNum: any[];
  roomRegisterForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>; // vaka mozam da sostapam do site Configs, opcii na primer max date i color /

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

  createRegisterForm() {
    this.roomRegisterForm = this.fb.group({
      number: ['', Validators.required],
      floor: ['', Validators.required],
      beds: ['', Validators.required],
      stringstudent: ['', Validators.required],
    });
  }

  registerRoom() {
    // treba da gi zemam site vrednosti od spavo i da gi stavam vo api user
    if (this.roomRegisterForm.valid) {
      //  ako e VALID // 132
      this.room = Object.assign({}, this.roomRegisterForm.value);
      const notANumberArr = this.room.stringstudent.split(',').map(Number);
      console.log('norANUmberArr = ' + notANumberArr);
      this.room.StudentIds = notANumberArr;

      this.userService
        .CreateRoom(this.authService.decodedToken.nameid, this.room)
        .subscribe(
          () => {
            this.aletify.success('uspesno registrirana soba');
            this.router.navigate(['/rooms']);

          },
          (error) => {
            this.aletify.error('The Register failed');
          }, () => {
            this.authService.login(this.room).subscribe(() => {
              this.router.navigate(['/rooms']);
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
