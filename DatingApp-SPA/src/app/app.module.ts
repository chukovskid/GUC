import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http'

import { AppComponent } from './app.component';
import { ValueComponent } from './value/value.component';


@NgModule({
   declarations: [
      AppComponent,
      ValueComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule // so ova  ke gi zimame podatocite od DB. za da se ukluci mora gore manualno da se pise '@angular/common/http'

   ],
   providers: [],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
