import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { ListsResolver } from './_resolvers/lists.resolver';
import { MessagesResolver } from './_resolvers/messages.resolver';

/*
Add routing paths from nav bar
Add Route guards from unauthorized users
allow anon access to home (only)

children routing get id via :id via [routeLink]

*/
export const appRoutes: Routes = [
    {   path: '', component: HomeComponent  },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children:   [
            {   path: 'members', component: MemberListComponent, resolve: {users: MemberListResolver} },
            {   path: 'members/:id', component: MemberDetailComponent, resolve: {user: MemberDetailResolver} },
            {   path: 'member/edit', component: MemberEditComponent, resolve: {user: MemberEditResolver},
                 canDeactivate: [PreventUnsavedChanges] },
            {   path: 'messages', component: MessagesComponent , resolve : {messages: MessagesResolver} },
            {   path: 'lists', component: ListsComponent , resolve: {users: ListsResolver} }
        ]
    },
    {   path: '**', redirectTo: 'home', pathMatch: 'full'}

];
