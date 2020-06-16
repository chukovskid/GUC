import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';
import { AlertifyService } from '../../_services/alertify.service';
import { User } from '../../_models/user';
import { ActivatedRouteSnapshot, ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  users: User[];
  user: User = JSON.parse(localStorage.getItem('user')); // za gender ke go koristam
  genderList = [{value: 'male', display: 'Males'}, {value: 'female', display: 'Femals'}]; // 147
  userParams: any = {};
  pagination: Pagination;
  constructor(
    private userService: UserService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      // zemi go toa od ActivatedRoute
      this.users = data['users'].result;
       // ^ stavi go u ovoj.user od ng (gore definiran) // 143 // dodaden .result bidejki vrakjam i UsersParams pokraj Userite
      this.pagination = data['users'].pagination; //  ne gi sfakjam ?
      const totalItems = this.pagination.totalItems;

      this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female'; // ako e female, vrati male vo sprotivno obrano
      this.userParams.minAge = 18;
      this.userParams.maxAge = 99;
      this.userParams.orderBy = 'lastActive';

    });
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }
resetFilters(){ // 147
  this.userParams.gender = this.user.gender === 'female' ? 'male' : 'female'; // ako e female, vrati male vo sprotivno obrano
  this.userParams.minAge = 18;
  this.userParams.maxAge = 99;
  this.userParams.orderBy = 'lastActive';
  this.loadUsers();

}
  loadUsers(){
s    this.userService.getUsers(this.pagination.currentPage, this.pagination.itemsPerPage, this.userParams)
    .subscribe((res: PaginatedResult<User[]>) => { // getUsers() vo user.service go vrakja PaginatedResult od subscribe
      this.users = res.result; // postavuvam deka  ovoj users da gi zeme vrednostite od subscribe.users
      this.pagination = res.pagination;
       }, error => {
         this.alertify.error(error);
       });
  }


}
