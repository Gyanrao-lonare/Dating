import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_modal/photo.interface';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-management',
  templateUrl: './photo-management.component.html',
  styleUrls: ['./photo-management.component.css'],
})
export class PhotoManagementComponent implements OnInit {
  photos: Photo;
  constructor(private adminService: AdminService) {}

  ngOnInit(): void {
    this.getPhotos();
  }

  getPhotos() {
    this.adminService.getPhotosForAprove().subscribe((response: any) => {
      var filterdata = response.filter((x) => !x.isAproved);

      this.photos = filterdata;
    });
  }
  aprovePhoto(id: number) {
    this.adminService.aprovePhoto(id).subscribe(
      (photo) => {
        this.getPhotos();
        console.log(photo);
      },
      (err) => {
        if (err.status == 200) {
          this.getPhotos();
        }
      }
    );
  }
}
