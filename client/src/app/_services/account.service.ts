import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment.prod';
import { User } from '../_modal/user.interface';
import { PresenceService } from './presence.service';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  baseUrl = environment.url;
  private currentUserSource = new ReplaySubject();
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient,private toast:ToastrService,private route: Router,private precenceService : PresenceService) {}
  login(user: any) {
    return this.http
      .post(this.baseUrl + 'Account/login', user)
      .subscribe((Response:User) => {
        
        this.currentUserSource.next(Response);
        this.precenceService.createHubConnection(user);
        localStorage.setItem('user', JSON.stringify(Response));
        this.route.navigateByUrl("home");
      },(error:any)=>{
        this.toast.error(error.error);
      });
  }
    setCurrentUser(user:User){
      user.roles = [];
      const roles = this.getDecodedToken(user.token).role;
      Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }
  logOut(){
    this.currentUserSource.next(null);
    localStorage.removeItem('user');
    this.precenceService.stopHubConnection();
    localStorage.clear();
    this.route.navigateByUrl("home")

  }
  register(user:any){
    return this.http
    .post(this.baseUrl + 'Account/register', user)
    .pipe()
    .subscribe((Response:User) => {
      localStorage.setItem('user', JSON.stringify(Response));
      this.currentUserSource.next(Response);
    this.precenceService.createHubConnection(user);
      this.route.navigateByUrl("home")
    },(error:any)=>{
      this.toast.error(error.error);
    } );  
  }  
  getDecodedToken(token) {
    return JSON.parse(atob(token.split('.')[1]));
  }

}
