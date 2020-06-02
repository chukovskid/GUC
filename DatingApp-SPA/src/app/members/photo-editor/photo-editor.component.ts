import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Photo } from 'src/app/_models/photo';
import { environment } from 'src/environments/environment'; // 84
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';



@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
@Input() photos: Photo[];
@Output() getMemberPhotoChange = new EventEmitter<string>(); // INSTANTEN Output od funkcijava za Instantno da smeni i profilna slika u EDIT(ima zosto e Instantno! potencirano)

  uploader: FileUploader; // 111
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  currentMain: Photo;


  constructor(private authService: AuthService, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }


   fileOverBase(e: any): void { // 111
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader(){ // proverka za koj user e i uste nekolku detali // 111
    this.uploader = new FileUploader({
      // localhost:5000/api/ + users/ + id + /photos
      url: this.baseUrl + 'users/' + this.authService.decodedToken.nameid + '/photos',
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false, // nejkam da e auto poso sakam da KLIKNAT kopce za Upload
      maxFileSize: 10 * 1024 * 1024 // max  golemina
    });

    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; };

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response){
        const res: Photo = JSON.parse(response); // go pravi Object! response
        const photo = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.photos.push(photo); // bidejki pravam push i racno ja dodavam slikava neli pri reload treba
        // uste edna da se dodade? Dime razmisli go ova
        if (photo.isMain){
        this.authService.changeMemberPhoto(photo.url); // changeMemberPhoto menuva na nav, u cards i details Main PHOTO // 133
        this.authService.currentUser.photoUrl = photo.url; // ovaa promenliva e samo za da ja iskoristam kako overide na localStorage user
        localStorage.setItem('user', JSON.stringify(this.authService.currentUser)); // 133

        }
      }
    };
  }

  deletePhoto(id: number){ // photoId
    this.alertify.confirm('Do you want to delete this photo ?', () => {
      this.userService.deletePhoto(this.authService.decodedToken.nameid, id).subscribe(() => { // izbrisi vo userService
        this.photos.splice(this.photos.findIndex(p => p.id === id), 1); // delete vo photos (id)
        this.alertify.success('You have deleted this photo');
      }, error => {
        console.log(this.authService.decodedToken.nameid);
        this.alertify.error(error);
      });
    });
  }

  setMainPhoto(photo: Photo) {  // funkc na photo-editor kade ke mu pratam u html photo od for(otvori html)
    // dole treba u setMainPhoto (od UserService) prima (userId i photo.id)
    this.userService.setMainPhoto(this.authService.decodedToken.nameid, photo.id).subscribe(() => {
        this.currentMain = this.photos.filter(p => p.isMain === true)[0]; // bidejki vrakja Arey mora [0]
        this.currentMain.isMain = false;
        photo.isMain = true;

        // this.getMemberPhotoChange.emit(photo.url); /// getMemberPhotoChange e New EMITER koj ke Emitne output na photo.url
        this.authService.changeMemberPhoto(photo.url); // ja smeniv so prethodnata funk bidejki Instantno sakam da se smeni slikata 118

        this.authService.currentUser.photoUrl = photo.url; // ovaa promenliva e samo za da ja iskoristam kako overide na localStorage user
        localStorage.setItem('user', JSON.stringify(this.authService.currentUser)); // 118
    }, error => {
      this.alertify.error(error);
    });

  }

}
