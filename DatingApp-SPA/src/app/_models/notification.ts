import { User } from './user';
import { NotificationUser } from './notificationUser';

export interface Notification {
    id: number;
    title: string;
    description: string;
    created: Date;
    content: string;
    readBy: User[];
    readByCount: number;
    notificationUser: NotificationUser[];
}
