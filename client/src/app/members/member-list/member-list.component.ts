import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { member } from 'src/app/_modal/member.interface';
import { Pagination } from 'src/app/_modal/pagination';
import { User } from 'src/app/_modal/user.interface';
import { userParms } from 'src/app/_modal/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members$:Observable<member[]>;
  members : member[];
  pagination : Pagination;
  userParams :userParms;
  user:User;
  genderList = [{value:"male",display:"Males"},{value:"female",display:"Females"}]
  constructor(private memberService: MemberService , private accountService:AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe((user :any)=>
      {
        this.user = user;
        this.userParams = new userParms(user);
      }
      )
  }

  ngOnInit(): void {
  // this.members$ = this.memberService.getMembers();  
this.loadMembers();
}

  loadMembers(){
    
    this.memberService.getMembers(this.userParams).subscribe((response:any)=>{
      this.members = response.result;
      
      this.pagination = response.pagination;
    })
  }
  
  pageChanged(event:any){
    this.userParams.pageNumber = event.page;
    this.loadMembers();
  }
  resetFilter(){
    this.userParams = new userParms(this.user);
    this.loadMembers();
  }

}
