import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Message } from '../_modal/Message';
import { Pagination } from '../_modal/pagination';
import { ConfirmService } from '../_services/confirm.service';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  members$:Observable<Message[]>;
messages : Message[];
pagination : Pagination;
container = "Unread";
pageNumber = 1;
pageSize = 5;
  constructor(private messageService : MessageService, private confirmService:ConfirmService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages(){
    // this.container = containerval
    this.messageService.getMessages(this.pageNumber,this.pageSize, this.container).subscribe(( res:any )=>{
      this.messages = res.result;
      this.pagination = res.pagination;
    });
  }

  deleteMessage(id:number){
    this.confirmService.confirm('Confirm delete message', 'This cannot be undone').subscribe(result => {
      if (result) {
        this.messageService.deleteMessage(id).subscribe(() => {
          this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
        })
      }
    })
    // this.messageService.deleteMessage(id).subscribe(res=>{
    //   this.messages = this.messages.filter(x=>x.id != id);
    // })
  }
  pageChanged(event:any){
    this.pageNumber = event.page;
    this.loadMessages();
  }

}
