import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { map, of } from 'rxjs';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const router = inject(Router);

  // we want to get to checkout after login
  // work with observables, since they are asyn
  if (accountService.currentUser()) {
    return of(true);    // route guard will wait until this observable is complete and then move on
  } else {
    return accountService.getAuthState().pipe(
      map(auth => {
        if (auth.isAuthenticated) {
          return true;    // this will return an observable
        } else {
            router.navigate(['/account/login'], {queryParams: {returnUrl: state.url}})
            return false;
        }
      })
    )
  }
};
