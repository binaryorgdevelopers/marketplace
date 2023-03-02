import { Component, OnInit } from '@angular/core';
import {Router} from "@angular/router";
import {FormBuilder, Validators} from "@angular/forms";
import VanillaTilt from "vanilla-tilt";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  password:  number | undefined;
  username: string | undefined;
  userLoginForm = this.fb.group({
    username: ['', Validators.required],
    password: ['', Validators.required]
  });

  constructor(
    private router: Router,
    private fb: FormBuilder,
  ) {
  }

  ngOnInit(): void {


    // Utils.clearSesionStorage();

    setTimeout(() => {
      VanillaTilt.init(document.querySelectorAll('.login100-pic') as any);
    }, 1);

  }


  ngOnDestroy(): void {
  }



  signIn() {
    if (this.userLoginForm.invalid) {
      this.userLoginForm.markAllAsTouched();

      return;
    }
    console.log('submitted');
    const login = this.userLoginForm.value;

    const body = {
      username: login.username,
      password: login.password,
    };

    // this.authService.login(body).subscribe(res => {
    //   // Utils.setToSessionStorage('login', res);
    //   // this.router.navigate([`${Route.Report}`]);
    //
    // }, (error: any) => {
    //   console.log('appLogin:', error);
    //   if (error.status === 400 || error.status === 404) {
    //     // ErrorResponse.ErrorResponse(error.status, error.error.error);
    //   }
    //
    // });


  }

}
