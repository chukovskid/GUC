import { Injectable } from '@angular/core';
import * as alertify from 'alertifyjs';

@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }


confirm(message: string, okCallback: () => any){
  alertify.confirm(message, (e: any) => { // na event (izgleda kakov bilo poso ANY pisuva)
    if (e) { // i ako stisnal OK ke se desi naredno
      okCallback();
    } else{}
  });
}

success(message: string){
  alertify.success(message);
}
error(message: string){
  alertify.error(message);
}
warning(message: string){
  alertify.warning(message);
}
message(message: string){
  alertify.message(message);
}


}
