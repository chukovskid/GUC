import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-notifications-list',
  templateUrl: './notifications-list.component.html',
  styleUrls: ['./notifications-list.component.css']
})
export class NotificationsListComponent implements OnInit {
  notifications: any;
  readByCount: number;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.notifications = data['notifications'].result;
    });
    // console.log(this.notifications);

    this.loadNotifications();

  }


  loadNotifications() {
    this.userService.getNotifications(this.authService.decodedToken.nameid).subscribe(
      (notifications) => {
        this.notifications = notifications;
      },
      (error) => {
        this.alertify.error(error);
      }
    );

    // this.ReadByCount(this.notifications);

  }


  AddAReader(notificationId: number){
    console.log('stigna do AddAReader');
    this.userService.sendARead(this.authService.decodedToken.nameid, notificationId );

  }
  // ReadByCount(notifications) {
  //   notifications.forEach(notification => {
  //     notification.readByCount = notification.readBy.lenght;
  //     console.log(notification.readByCoun);

  //   });
  // }
}
