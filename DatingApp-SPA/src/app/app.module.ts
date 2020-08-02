import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { FileUploadModule } from 'ng2-file-upload';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TimeagoModule } from 'ngx-timeago';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';







import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { ListsComponent } from './lists/lists.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { appRoutes } from './routes';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { AuthGuard } from './_guards/auth.guard';
import { UserService } from './_services/user.service';
import { AlertifyService } from './_services/alertify.service';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/hasRole.directive';
import { RoomRegisterComponent } from './rooms/room-register/room-register.component';
import { RoomDetailsComponent } from './rooms/room-details/room-details.component';
import { RoomResolver } from './_resolvers/room.resolver';
import { RoomsResolver } from './_resolvers/rooms.resolver';
import { RoomComponent } from './roomList/room.component';
import { NotificationResolver } from './_resolvers/notification.resolver';
import { NotificationsResolver } from './_resolvers/notifications.resolver';
import { NotificationDetailsComponent } from './notifications/notification-details/notification-details.component';
import { NotificationCreateComponent } from './notifications/notification-create/notification-create.component';
import { NotificationsListComponent } from './notifications/notifications-list/notifications-list.component';



export function tokenGetter() {
   return localStorage.getItem('token'); // 89
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      ListsComponent,
      MemberListComponent,
      MessagesComponent,
      MemberCardComponent,
      MemberDetailComponent,
      MemberEditComponent,
      MemberMessagesComponent,
      PhotoEditorComponent,
      AdminPanelComponent,
      HasRoleDirective,
      RoomRegisterComponent,
      RoomDetailsComponent,
      RoomComponent,
      NotificationDetailsComponent,
      NotificationCreateComponent,
      NotificationsListComponent,
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      PaginationModule.forRoot(),
      BsDatepickerModule.forRoot(),
      FileUploadModule,
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      ButtonsModule.forRoot(), // 149
      TabsModule.forRoot(),
      TimeagoModule.forRoot(),  // 136
      NgxGalleryModule, // 94
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']

         }
      })
   ],
   providers: [
      ErrorInterceptorProvider,
      AuthService,
      AuthGuard,
      PreventUnsavedChanges,
      UserService,
      AlertifyService,
      MemberDetailResolver,
      MemberListResolver,
      MemberEditResolver,
      ListsResolver,
      RoomsResolver,
      RoomResolver,
      MessagesResolver,
      NotificationResolver,
      NotificationsResolver,

   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
