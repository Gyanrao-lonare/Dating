import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
registerModal:any ={}
registerForm : FormGroup;
maxDate :Date;
  constructor(public route:Router, private accountService:AccountService,private fb : FormBuilder, private toastr:ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate = new Date();
    this.maxDate.setFullYear(this.maxDate.getFullYear()-18);

  }

  initializeForm(){
    this.registerForm = this.fb.group({
      username: ['',Validators.required],
      KnownAs: ['',Validators.required],
      dateOFBirth: ['',Validators.required],
      gender: ['male',Validators.required],
      city: ['',Validators.required],
      country: ['',Validators.required],
      password: ['',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
      confirmPassword: ['',[Validators.required,this.matchValues('password')]],
    })
    this.registerForm.controls.confirmPassword.valueChanges.subscribe(()=>{
      this.registerForm.controls.password.updateValueAndValidity();
    })
      }


    
  matchValues(matchTo:string):ValidatorFn{
    return(control:AbstractControl)=>{
      return control?.value === control?.parent?.controls[matchTo].value ? null :{isMaching : true}
    }
  }
  register(){
    if(this.registerForm.status === "VALID"){
      this.accountService.register(this.registerForm.value);
      }
  }
  cancel(){
    this.route.navigateByUrl("");
  }

}
