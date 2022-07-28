import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import {
  NgxGalleryAnimation,
  NgxGalleryImage,
  NgxGalleryOptions,
} from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { take } from 'rxjs';
import { member } from 'src/app/_modal/member.interface';
import { Message } from 'src/app/_modal/Message';
import { User } from 'src/app/_modal/user.interface';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
})
export class MemberDetailComponent implements OnInit , OnDestroy, AfterViewInit{
  @ViewChild('memberTabs') memberTabs: TabsetComponent;
  member: any;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[] = [];
  activeTab: TabDirective;
  messages: Message[] = [];
  user: User;
  constructor(
    public presence: PresenceService,
    private memberService: MemberService,
    private route: ActivatedRoute,
    private messageService: MessageService,
    private accountService: AccountService,
    private router: Router
  ) {   
    this.route.data.subscribe((member:any)=>{
      this.member = member.member
    })
    this.accountService.currentUser$.pipe(take(1)).subscribe((user:any) => this.user = user);
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
 }
  ngAfterViewInit(): void {
    this.route.queryParams.subscribe((params) => {
      params.tab ? this.selectTab(parseInt(params.tab)) : this.selectTab(0);
    });
  }

  ngOnInit(): void {
    

    this.galleryOptions = [
      {
        width: '600px',
        height: '400px',
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
      },
      // max-width 800
      {
        breakpoint: 800,
        width: '100%',
        height: '600px',
        imagePercent: 80,
        thumbnailsPercent: 20,
        thumbnailsMargin: 20,
        thumbnailMargin: 20,
      },
      // max-width 400
      {
        breakpoint: 400,
        preview: false,
      },
    ];
    this.imageGenrator();
  }

  likeUser(name) {
    this.memberService.likeUser(name);
  }
  imageGenrator() {
    this.member.photos.forEach((item) => {
      this.galleryImages.push({
        small: item.url,
        medium: item.url,
        big: item.url,
      });
    });
  }
  loadMessageThread() {
    this.messageService
      .getMessageThread(this.member.userName)
      .subscribe((message: any) => {
        this.messages = message;
      });
  }
  selectTab(tabId: number) {
    if(this.memberTabs){
    this.memberTabs.tabs[tabId].active = true;
  }
  }
  onTabActivate(data: TabDirective) {
    this.activeTab = data;
    
    if (this.activeTab.heading == 'Messages' && this.messages.length === 0) {
      this.messageService.createHubConnection(this.user, this.member.userName);
    }else{
      this.messageService.stopHubConnection();
    }
  }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
