import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { PaginatedResult } from '../_models/pagination';
import { BusyService } from './busy.service';

@Injectable({
  providedIn: 'root'
})
export class SharedService {

  constructor(public http: HttpClient, public toastr: ToastrService, public router: Router, public busyService: BusyService) { }


  public setUser(model: any){
    localStorage.setItem('user', JSON.stringify(model));
  }

  public getUser(){
    return JSON.parse(localStorage.getItem('user'));
  }

  public removeUser(){
    localStorage.removeItem('user');
  }

  public baseUrl = 'https://localhost:5001/api/'; //If doing things in the long run, consider using environ variables

  public hasApplication: boolean;
  public siteTelephone: string = "3214567";

  /**
   * The below  methods are being used by messages but I'm putting them here cause they can be used in other places.
   * You will also find them in their own files and stuff but I put them here cause intellisense wasn't picking them up and I wanted
   * to move along and not waste too much time. Consider removing this and using those for future maintainance
   * @param url
   * @param params
   * @param http
   */

  public getPaginatedResult<T>(url, params, http: HttpClient) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') !== null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }
        return paginatedResult;
      })
    );
  }

   public getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();

    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());

    return params;
  }

}
