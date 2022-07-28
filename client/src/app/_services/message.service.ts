import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Group } from '../_modal/group';
import { Message } from '../_modal/Message';
import { User } from '../_modal/user.interface';
import { BusyService } from './busy.service';
import { getPaginatedResults, getPaginationHEaders } from './PaginationHelper';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  baseUrl = environment.url;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();

  constructor(private http: HttpClient , private busyService: BusyService) {}

  
  createHubConnection(user: User, otherUsername: string) {
    
    this.busyService.busy();
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'message?user=' + otherUsername, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build()

    this.hubConnection.start()
      .catch(error => console.log(error))
      .finally(() => this.busyService.idly());

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThreadSource.next(messages);
    })

    this.hubConnection.on('NewMessage', message => {
      this.messageThread$.pipe(take(1)).subscribe(messages => {
        this.messageThreadSource.next([...messages, message])
      })
    })

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some(x => x.username === otherUsername)) {
        this.messageThread$.pipe(take(1)).subscribe(messages => {
          messages.forEach(message => {
            if (!message.dateRead) {
              message.dateRead = new Date(Date.now())
            }
          })
          this.messageThreadSource.next([...messages]);
        })
      }
    })
  }

  stopHubConnection() {
    if (this.hubConnection) {
      this.messageThreadSource.next([]);
      this.hubConnection.stop();
    }
  }
  getMessages(pageNumber, pageSize, container) {
    
    let param = getPaginationHEaders(pageNumber, pageSize);
    param = param.append('Container', container);
    return getPaginatedResults<Message>(this.baseUrl + 'Message', param, this.http);
  }
  getMessageThread(username:string){
  return  this.http.get<Message>(this.baseUrl + "Message/thred/" + username);
  }
 async sendMessage(username: string, content: string) {
  
    return this.hubConnection.invoke('SendMessage', {recipientUsername: username, content})
      .catch(error => console.log(error));
  //  return this.http.post(this.baseUrl + "Message", param);
  }
  deleteMessage(id:number){
   return this.http.delete(this.baseUrl + "Message/" + id);
  }
}
