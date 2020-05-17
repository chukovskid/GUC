import {
  Component,
  OnInit,
  ViewChild,
  Host,
  HostListener,
} from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css'],
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  user: User;
  @HostListener('window:beforeunload', ['$event']) // 100 za pri gasenje na tab da javi alert za Unsaved
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private userService: UserService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.user = data['user'];
    });
  }

  updateUser() {
    this.userService
      .updateUser(this.authService.decodedToken.nameid, this.user) // go koristam updateUser OD UserService!! i mu davam id od Auth a user istiot
      .subscribe((next) => {
        this.alertify.success('Update is made');
        this.editForm.reset(this.user); // ova treba da gi vrati sea kako sto se sejvnati
      }, error => {
        this.alertify.error(error);
      });
  } //  99 finta ima vo htmlot bidejki formata sto ja koristi funkcijava ne e u nejze submit buttonot / otvori vidi
}
