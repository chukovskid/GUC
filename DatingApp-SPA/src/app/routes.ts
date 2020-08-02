import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { RoomsResolver } from './_resolvers/rooms.resolver';
import { RoomRegisterComponent } from './rooms/room-register/room-register.component';
import { RegisterComponent } from './register/register.component';
import { RoomResolver } from './_resolvers/room.resolver';
import { RoomDetailsComponent } from './rooms/room-details/room-details.component';
import { RoomComponent } from './roomList/room.component';
import { NotificationsListComponent } from './notifications/notifications-list/notifications-list.component';
import { NotificationsResolver } from './_resolvers/notifications.resolver';
import { NotificationCreateComponent } from './notifications/notification-create/notification-create.component';
import { NotificationDetailsComponent } from './notifications/notification-details/notification-details.component';
import { NotificationResolver } from './_resolvers/notification.resolver';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent},
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'lists', component: ListsComponent, resolve: {users: ListsResolver}}, // 93
            { path: 'messages', component: MessagesComponent, resolve: {messages: MessagesResolver}},
            { path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver}},
            { path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver},
             canDeactivate: [PreventUnsavedChanges]},
            { path: 'members/:id', component: MemberDetailComponent, resolve:  {user: MemberDetailResolver}}, // 93
            { path: 'admin', component: AdminPanelComponent, data: {roles: ['Admin', 'Moderator']}}, // 211
            { path: 'rooms', component: RoomComponent, resolve: {rooms: RoomsResolver}},
            { path: 'rooms/register', component: RoomRegisterComponent},
            { path: 'rooms/:id', component: RoomDetailsComponent, resolve: {room: RoomResolver}},
            { path: 'register', component: RegisterComponent},
            { path: 'notifications', component: NotificationsListComponent, resolve: {notifications: NotificationsResolver}},
            { path: 'notifications/register', component: NotificationCreateComponent},
            { path: 'notifications/:id', component: NotificationDetailsComponent, resolve: {notification: NotificationResolver}},

            // { path: 'members', component: MemberListComponent, canActivate: [AuthGuard]}


        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full'}
];
