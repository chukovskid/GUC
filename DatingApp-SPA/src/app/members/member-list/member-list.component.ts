import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { User } from '../../_models/user';
import { ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  users: User[];
  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      // zemi go toa od ActivatedRoute
      this.users = data['users']; // stavi go u ovoj.user od ng (gore definiran)
    });
  }

  // loadUsers(){
  //   this.userService.getUsers().subscribe((users: User[]) => { // ova e so vrakjame od subscribe metodot
  //     this.users = users; // postavuvam deka  ovoj users da gi zeme vrednostite od subscribe.users
  //      }, error => {
  //        this.alertify.error(error);
  //      });
  // }
}
