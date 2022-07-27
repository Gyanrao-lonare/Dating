import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { member } from '../_modal/member.interface';
import { PaginationResult } from '../_modal/pagination';
import { userParms } from '../_modal/userParams';
import { getPaginatedResults, getPaginationHEaders } from './PaginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MemberService {
  baseUrl = environment.url;
  members: member[] = [];
  constructor(private http: HttpClient) {}
  getMembers(userParms: userParms) {
    ;
    let params = getPaginationHEaders(userParms.pageNumber, userParms.pageSize);

    params = params.append('minAge', userParms.minAge.toString());
    params = params.append('maxAge', userParms.maxAge.toString());
    params = params.append('gender', userParms.gender);
    return getPaginatedResults<member>(
      this.baseUrl + 'users',
      params,
      this.http
    );
  }
  
  

  // end

  getMember(userName: string) {
    const member = this.members.find((x) => x.userName == userName);
    if (member !== undefined) return of(member);
    return this.http.get<any>(this.baseUrl + 'users/' + userName);
  }
  updateUser(member: member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map((res) => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/add-main-photo/' + photoId,{});
  }

  likeUser(username: string) {
    return this.http
      .post(this.baseUrl + 'Likes/' + username, {})
      .subscribe((res) => {
        console.log(res);
      });
    // return this.http.post(this.baseUrl + 'Likes/' + username).subscribe(res=>{
    //   console.log(res);
    // });
  }
  getLikedUser(predicate: string, pageNumber: number, pageSize: number) {
    let parms = getPaginationHEaders(pageNumber, pageSize);
    parms = parms.append('predicate', predicate);
    return getPaginatedResults<Partial<member[]>>(
      this.baseUrl + 'likes',
      parms,
      this.http
    );
    // return this.http.get<Partial<member[]>>(this.baseUrl + 'Likes?predicate='+ predicate);
  }
}
