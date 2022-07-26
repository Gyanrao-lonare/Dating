import { Inject, Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  Resolve,
  RouterStateSnapshot,
} from '@angular/router';
import { Observable } from 'rxjs';
import { member } from '../_modal/member.interface';
import { MemberService } from '../_services/member.service';

@Injectable({
  providedIn: 'root',
})
export class MemberDetailResolver implements Resolve<member> {
  constructor(private memberService: MemberService) {}
  resolve(route: ActivatedRouteSnapshot): Observable<member> {
    return this.memberService.getMember(route.paramMap.get('username'));
  }
}
