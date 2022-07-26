import { User } from './user.interface';

export class userParms {
  gender: string;
  minAge:number = 18;
  maxAge:number= 99;
  pageNumber:number= 1;
  pageSize:number= 5;

  constructor(user: User) {
    this.gender = user.gender === 'female' ? 'male' : 'female';
  }
}
