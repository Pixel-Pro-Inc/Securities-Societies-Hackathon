import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {
  signupForm: FormGroup
  validators: string[] = [];

  firstdone = false;
  showOmang = false;

  constructor(private fb: FormBuilder, private accountService: AccountService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(){
    this.signupForm = this.fb.group({
      firstname: ['', [Validators.required, this.containsNumbers()]],
      lastname: ['', [Validators.required, this.containsNumbers()]],
      email: ['', [Validators.required, Validators.email]],
      phonenumber: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(8), this.isNumber()]],
      dateofbirth: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]]
    });

    this.signupForm.controls.password.valueChanges.subscribe(() => {
      this.signupForm.controls.confirmPassword.updateValueAndValidity();
    });

    this.signupForm.controls.dateofbirth.valueChanges.subscribe(() => {
      this.signupForm.controls.dateofbirth.updateValueAndValidity();
    });
  }

  form1Complete(): boolean{
    if(this.signupForm.controls['firstname'].errors != null 
    || this.signupForm.controls['lastname'].errors != null
    || this.signupForm.controls['dateofbirth'].errors != null){
      return true;
    }

    return false;
  }

  toggleShowDone(){
    this.firstdone = !this.firstdone;
  }

  toggleShowOmangForm(){
    this.showOmang = true;
  }


  containsNumbers(): ValidatorFn{
    return (control: AbstractControl) => {
      return (/^[A-Za-z]+$/).test(control?.value)? null : control?.value == ''? null : {hasNumbers: true};
    };
  }

  isNumber(): ValidatorFn{
    return (control: AbstractControl) => {
      return isNaN(control?.value) ? {hasLetters: true} : null;
    };
  }

  matchValues(macthTo: string): ValidatorFn{
    return (control: AbstractControl) => {
      return control?.value === control?.parent?.controls[macthTo].value ? null : {isMatching: true};
    };
  }

  createAccount(){
    this.accountService.signup(this.signupForm.value);
  }

  onFileChange(event: any) {
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);

      reader.onload = () => {
        this.omangFill(reader.result as string);
      };

    }
  }

  omangFill(img: string){
    let response: any = {};

    this.accountService.omangFill(img).subscribe(r => {
      response = r;

      this.signupForm.controls['firstname'].setValue(response.firstname);
      this.signupForm.controls['lastname'].setValue(response.lastname);
      this.signupForm.controls['dateofbirth'].setValue(response.dateofbirth);
    });
  }

}
