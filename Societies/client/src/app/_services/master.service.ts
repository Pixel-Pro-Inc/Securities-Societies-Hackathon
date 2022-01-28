import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MasterService {

  constructor() { }

  hasApplication: boolean;
  siteTelephone: string = "3214567";
}
