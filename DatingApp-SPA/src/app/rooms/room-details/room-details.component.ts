import { Component, OnInit, ViewChild } from '@angular/core';
import { Room } from 'src/app/_models/room';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { TabsetComponent } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-room-details',
  templateUrl: './room-details.component.html',
  styleUrls: ['./room-details.component.css'],
})
export class RoomDetailsComponent implements OnInit {
  @ViewChild('memberTabs', { static: true }) memberTabs: TabsetComponent;
  students: User[];
  room: Room;
  student: User;
  constructor(
    private alertify: AlertifyService,
    private userService: UserService,
    private route: ActivatedRoute
  ) {}
  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.room = data['room'];
    });
    // this.route.data.subscribe((data) => {
    //   this.students = data['users'];
    // });

    // this.getStudents(this.room.StudentIds);
  }

  // filterUsers(students){
  //   students.forEach(student => {
  //     if (student.roomId === this.room.id) {
  //       this.roomStudents.push(student);
  //     }
  //   });
  // }

  getStudents(studentIds: number[]) {
    console.log(studentIds);
    studentIds.forEach((studentId) => {
      this.userService.getUser(studentId).subscribe((student) => {
        this.students.push(student);
      });
    });
  }

  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }
}
