import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
   loginModal : any ={} 
  constructor(public accountService:AccountService) { }

  ngOnInit(): void {
  }
  onlogin(){
    this.accountService.login(this.loginModal);
console.log(this.loginModal);
  }
  logOut(){
    this.accountService.logOut();
  }

}
