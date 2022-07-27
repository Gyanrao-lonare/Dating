import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { MemberService } from 'src/app/_services/member.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css'],
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: any;
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  hasAnotherDropZoneOver: boolean;
  response: string;
  baseUrl = environment.url;
  user: any;
  urls: any;
  constructor(private accountService: AccountService, private memberService:MemberService,
     private toast:ToastrService,private toastr : ToastrService) {
    this.accountService.currentUser$
      .pipe(take(1))
      .subscribe((user) => (this.user = user));
  }

  
  ngOnInit(): void {
    this.initializeUploader();
  }
  public fileOverBase(e: any): void {
    
    this.hasBaseDropZoneOver = e;
  }
  initializeUploader() {
    
    this.uploader = new FileUploader({
      url: this.baseUrl + 'Users/add-photo',
      authToken: "Bearer " + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    };
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      
      if (response) {
        const photo = JSON.parse(response);
        this.member.photos.push(photo);
      }
    };
  }

  updateMember() {
    if (this.urls) {
      if(this.member.photos){
      this.member.photos.push(this.urls);
      }else{
        this.member["photos"] = [];
        this.member.photos.push(this.urls);
      }
      this.uploader.uploadAll();
    }
    this.memberService.updateUser(this.member).subscribe((responce) => {
        this.urls = undefined;
    });
  }
  setProfileImg(photoId:number){
    this.memberService.setMainPhoto(photoId).subscribe((responce) => {
      this.toastr.success('Main Photo Set  succefully');
      // this.editForm.reset(this.member);       
  });
  }
  // setProfileImg() {
  //   this.member['photoUrl'] = img;
  //   this.accountService.setCurrentUser(this.member);
  //   // this.updateMember();
  // }
  deleteImage(photoId:number) {

    this.memberService.deletePhoto(photoId).subscribe((response)=>{
      this.member.photos =  this.member.photos.filter(x=> x.id != photoId)
    this.toast.success("image Delete Succefully");
    })
    // this.member.photos.splice(i, 1);
    // this.updateMember();
  }


  public fileOverAnother(e: any): void {
    this.hasAnotherDropZoneOver = e;
  }
}
