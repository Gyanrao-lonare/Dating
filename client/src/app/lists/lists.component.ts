import { Component, OnInit } from '@angular/core';
import { member } from '../_modal/member.interface';
import { Pagination } from '../_modal/pagination';
import { MemberService } from '../_services/member.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css'],
})
export class ListsComponent implements OnInit {
  members: Partial<member[]>;
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination;

  constructor(private memberService: MemberService) {}

  ngOnInit(): void {
    this.loadMembers('liked');
  }
  loadMembers(type: string) {
    this.memberService
      .getLikedUser(type, this.pageNumber, this.pageSize)
      .subscribe(res=> {
        
        this.members = res.result;
        this.pagination = res.pagination;
      });
  }
  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadMembers('liked');
  }
}
