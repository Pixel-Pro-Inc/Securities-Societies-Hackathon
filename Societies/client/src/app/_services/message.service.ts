import { Injectable } from '@angular/core';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeaders } from './pagination-helper';
import { SharedService } from './shared.service';

@Injectable({
  providedIn: 'root'
})
export class MessageService {

  constructor(private shared: SharedService) { }

  getMessages(pageNumber, pageSize, container) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);

    return getPaginatedResult<Message[]>(this.shared.baseUrl + 'messages', params, this.shared.http);
  }



}
