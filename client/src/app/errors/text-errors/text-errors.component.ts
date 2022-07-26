import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-text-errors',
  templateUrl: './text-errors.component.html',
  styleUrls: ['./text-errors.component.css'],
})
export class TextErrorsComponent implements OnInit {
  baseUrl = 'https://localhost:5001/api/';
  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  get404Error() {
    this.http.get(this.baseUrl + 'BuggyErr/not-found').subscribe(
      (res) => {
        console.log(res);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  get401Error() {
    this.http.get(this.baseUrl + 'BuggyErr/auth').subscribe(
      (res) => {
        console.log(res);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  get500Error() {
    this.http.get(this.baseUrl + 'BuggyErr/server-error').subscribe(
      (res) => {
        console.log(res);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  get400Error() {
    this.http.get(this.baseUrl + 'BuggyErr/bad-request').subscribe(
      (res) => {
        console.log(res);
      },
      (error) => {
        console.log(error);
      }
    );
  }
  get400ValidationError() {
    this.http.get(this.baseUrl + 'BuggyErr/bad-request').subscribe(
      (res) => {
        console.log(res);
      },
      (error) => {
        console.log(error);
      }
    );
  }
}
