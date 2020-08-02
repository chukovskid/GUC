import { Component, OnInit } from '@angular/core';
import { Room } from '../_models/room';
import { AuthService } from '../_services/auth.service';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css'],
})
export class RoomComponent implements OnInit {
  rooms: any;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      // zemi go toa od ActivatedRoute
      this.rooms = data['rooms'].result;
    });
    this.loadRooms();
  }
  loadRooms() {
    this.userService.getRooms(this.authService.decodedToken.nameid).subscribe(
      (rooms) => {
        // getUsers() vo user.service go vrakja PaginatedResult od subscribe
        this.rooms = rooms; // postavuvam deka  ovoj users da gi zeme vrednostite od subscribe.users
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  bedList(numberOfBeds: number, occupiedBeds: number) {
    const bedList: number[] = new Array();
    for (let i = 1; i <= numberOfBeds; i++) {
      if (i > occupiedBeds) {
        bedList.push(1);
      } else {
        bedList.push(0);
      }
    }
    return bedList;
  }
}
