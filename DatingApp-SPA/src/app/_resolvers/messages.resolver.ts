import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import {    User} from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../_services/auth.service';
import { Message } from '../_models/message';

/* Resolver for non existent users */

@Injectable({ providedIn: 'root' })
export class MessagesResolver implements Resolve<Message[]> {
    /**
     * PIPE PAGE SIZE TO ASPNET
     */
    pageNumber = 1;
    pageSize = 5;
    messageContainer = 'Unread';

    constructor(private userService: UserService, private alertify: AlertifyService,
                private route: Router, private authService: AuthService) {
    }

    resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
        return this.userService.getMessages(this.authService.decodedToken.nameid,
            this.pageNumber, this.pageSize, this.messageContainer).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving messages');
                this.route.navigate(['/']);
                return of(null);
            })
        );
    }
}
