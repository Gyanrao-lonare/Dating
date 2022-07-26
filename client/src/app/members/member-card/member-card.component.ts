import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { member } from 'src/app/_modal/member.interface';
import { MemberService } from 'src/app/_services/member.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
@Input() member : member;
  constructor(private memberService:MemberService,private toastr: ToastrService, 
    public presence: PresenceService ) { }

  ngOnInit(): void {

  }

  likeUser(name){
    this.memberService.likeUser(name);
  }
}
