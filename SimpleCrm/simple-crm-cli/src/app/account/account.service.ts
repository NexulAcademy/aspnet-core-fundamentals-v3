import { PlatformLocation } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { anonymousUser, CredentialsViewModel, MicrosoftOptions, UserSummaryViewModel } from './account.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private baseUrl: string;
  private cachedUser = new BehaviorSubject<UserSummaryViewModel>(anonymousUser());

  constructor(
    private http: HttpClient,
    private router: Router,
    private platformLocation: PlatformLocation,

  ) {
    this.baseUrl = environment.server + environment.apiUrl + 'auth/';
    const cu = localStorage.getItem('currentUser');
    if (cu) {
      // if already logged in from before, just use that... it has a JWT in it.
      this.cachedUser.next(JSON.parse(cu));
    }
  }

  get user(): BehaviorSubject<UserSummaryViewModel> {
    return this.cachedUser;
  }
  setUser(user: UserSummaryViewModel): void {
    // called by your components that process a login from password, Google, Microsoft
    this.cachedUser.next(user);
    localStorage.setItem('currentUser', JSON.stringify(user));
  }


  public loginMicrosoftOptions(): Observable<MicrosoftOptions> {
    // TODO: it's up to you to add interface MicrosoftOptions to account.models.ts
    return this.http.get<MicrosoftOptions>(
      this.baseUrl + 'external/microsoft'
    );
  }

  /**
 * Name and password login API call.
 * If a successful login is completed, you may want to call loginComplete
 * to handle updates to the current user and redirect to where they
 * originally wanted to go.
 */
  public loginPassword(credentials: CredentialsViewModel): Observable<UserSummaryViewModel> {
    this.cachedUser.next(anonymousUser());
    localStorage.removeItem('currentUser');
    return this.http.post<UserSummaryViewModel>(this.baseUrl + 'login', credentials);
  }

  loginComplete(user: UserSummaryViewModel, _message: string) {
    this.setUser(user);
  }

  public loginMicrosoft(code: string, state: string): Observable<UserSummaryViewModel> {
    const body = { accessToken: code, state, baseHref: this.platformLocation.getBaseHrefFromDOM() };
    return this.http.post<UserSummaryViewModel>(this.baseUrl + 'external/microsoft', body);
  }

  /**
   * Removes any cached login data and sends the user to the main website home page.
   */
   public logout(options: {navigate: boolean} = {navigate: true}): void {
    this.cachedUser.next(anonymousUser());
    localStorage.removeItem('currentUser');
  }
}
