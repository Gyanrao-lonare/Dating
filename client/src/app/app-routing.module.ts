import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { TextErrorsComponent } from './errors/text-errors/text-errors.component';
import { HomeComponent } from './home/home.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { RegisterComponent } from './register/register.component';
import { ListComponent } from './users/list/list.component';
import { UsersModule } from './users/users.module';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailResolver } from './_resolver/member.detaled.resolver';

const routes: Routes = [
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'memberlist', component: MemberListComponent },
      { path: 'memberlist/:username', component: MemberDetailComponent,resolve:{member:MemberDetailResolver}},
      { path: 'lists', component: ListsComponent },
      { path: 'lists/:username', component: MemberDetailComponent,resolve:{member:MemberDetailResolver} },
      { path: 'messages', component: MessagesComponent },
      { path: 'admin', component: AdminPanelComponent,canActivate:[AdminGuard] },
    ],
  },
  { path: 'home', component: HomeComponent},
  { path: 'errors', component: TextErrorsComponent},
  { path: 'userprofile', component: MemberEditComponent, canDeactivate:[PreventUnsavedChangesGuard]},
  { path: 'register', component: RegisterComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', component: HomeComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
