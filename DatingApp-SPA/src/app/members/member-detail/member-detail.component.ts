import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { UserService } from 'src/app/_services/user.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { error } from 'protractor';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery-9';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit {
  user: User; // Spa User
  galleryOptions: NgxGalleryOptions[]; // 94
  galleryImages: NgxGalleryImage[];
  constructor(
    private alertify: AlertifyService,
    private userService: UserService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.user = data['user']; // ovoj user e od routes.ts pod /members/:id // 93
    });

    // tuka nadole e za Gallery 94



    this.galleryOptions = [
      {
          width: '600px',
          height: '400px',
          thumbnailsColumns: 4,
          imageAnimation: NgxGalleryAnimation.Slide
      },
    ];
    this.galleryImages = this.getImages();



    // this.loadUser();
  }

  getImages() {
    const imageUrls = [];
    for (const photo of this.user.photos) {

      imageUrls.push({

        small: photo.url,
        medium: photo.url,
        big: photo.url,
        description: photo.description,
      });

    }
    return imageUrls;
  }

  // loadUser(){
  //   // getUser vrakja observable zatoa mora subscribe // + kaj this e za Int da vrati od Id
  //   this.userService.getUser(+this.route.snapshot.params['id']).subscribe((user: User) => { // 90
  //     this.user = user;
  //   // tslint:disable-next-line: no-shadowed-variable
  //   }, error => {
  //     this.alertify.error(error);
  //   });
  // }
}
