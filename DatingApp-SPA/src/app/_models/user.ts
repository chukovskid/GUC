import { Photo } from './photo';
import { NotificationUser } from './notificationUser';

export interface User { // 83
id: number;
userName: string;
knownAs: string;
age: number;
gender: string;
created: Date;
lastActive: Date;
photoUrl: string;
city: string;
country: string;
interests?: string;
introduction?: string;
lookingFor?: string;
photos?: Photo[]; // povrzano so photo.ts interface
roomId: number;
notificationUser: NotificationUser[];
}
