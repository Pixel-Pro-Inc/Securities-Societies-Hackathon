import { Component } from '@angular/core';
import { SharedService } from './_services/shared.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'client';

  constructor(private shared: SharedService){
    this.shared.appComp = this;
  }

  mybutton = document.getElementById("myBtn");

// When the user clicks on the button, scroll to the top of the document
 topFunction() {
  document.body.scrollTop = 0; // For Safari
  document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
}

getMessage(){
  return localStorage.getItem('loadingMessage') + '...';
}

}
