import { User } from './user';

export interface Room {
  id: number;
  number: number;
  stringstudent: string;
  StudentIds: number[];
  floor: number;
  beds: number;
  occupiedBeds: number;
  roomId: number;
  student: User[];

}
