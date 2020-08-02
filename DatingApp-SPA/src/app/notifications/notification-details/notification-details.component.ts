import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { FormBuilder } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from 'src/app/_services/user.service';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-notification-details',
  templateUrl: './notification-details.component.html',
  styleUrls: ['./notification-details.component.css'],
})
export class NotificationDetailsComponent implements OnInit {
  notification: Notification;
  notif: any;
  readers: User[] = [];
  readersIds: number[] = [];

  constructor(
    private authService: AuthService,
    private userService: UserService,
    // private aletify: AlertifyService,
    private route: ActivatedRoute
  ) {}
  ngOnInit() {
    this.route.data.subscribe(
      (data) => (this.notification = data['notification'])
    );
    this.notif = this.notification;
    this.getReaders(this.notif);
  }

  getReaders(notif) {
    notif.notificationUsers.forEach((notification) => {
      this.readersIds.push(notification.readerId);
    });

    const allUsers = this.userService.getFullUsers();

    allUsers.forEach((user) => {

      const userot = user;
      userot.forEach((reader) => {
        this.readersIds.forEach((id) => {
          if (reader.id === id) {
            this.readers.push(reader);
          }
        });
      });
    });
  }
}
