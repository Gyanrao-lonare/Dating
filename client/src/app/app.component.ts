import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { User } from './_modal/user.interface';
import { AccountService } from './_services/account.service';
import { PresenceService } from './_services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'client';
  users: any;
  constructor(private http: HttpClient, private accountService:AccountService,private precenceService : PresenceService) {}
  ngOnInit() {
    this.setCurrentUser();
  }
  setCurrentUser(){ 
    const user = JSON.parse(localStorage.getItem('user')) ;
    if(user){
     this.accountService.setCurrentUser(user);
    this.precenceService.createHubConnection(user);
    }
}
}
