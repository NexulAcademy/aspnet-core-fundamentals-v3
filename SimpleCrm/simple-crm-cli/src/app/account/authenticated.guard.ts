import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { UserSummaryViewModel } from './account.model';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticatedGuard implements CanActivate {

  constructor(
    private router: Router,
    private accountService: AccountService,
  ){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

      return this.accountService.user.pipe(
        map((user: UserSummaryViewModel) => {
          if (user.name === 'Anonymous') {

            return this.router.createUrlTree(['./login']);
          }

          //TODO: check the user's roles

          return true;
        })
      );
  }

}
