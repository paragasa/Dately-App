import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import {    User} from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';

/* Resolver for non existent users */

@Injectable({ providedIn: 'root' })
export class MemberDetailResolver implements Resolve<User> {
    /**
     *
     */
    constructor(private userService: UserService, private alertify: AlertifyService,
                private route: Router) {
    }

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        return this.userService.getUser(route.params.id).pipe(
            catchError(error => {
                this.alertify.error('Problem Retrieving Data');
                this.route.navigate(['/members']);
                return of(null);
            })
        );
    }
}
