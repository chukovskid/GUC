import { User } from './user';

export interface NotificationUser {
    reader: User;
    notification: Notification;
    readerId: number;
    notificationId: number;

}
