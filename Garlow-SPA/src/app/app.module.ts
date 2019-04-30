import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BsDropdownModule, TabsModule } from 'ngx-bootstrap';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';
import { NgxGalleryModule } from 'ngx-gallery';
import { FileUploadModule } from 'ng2-file-upload';
import { ClipboardModule } from 'ngx-clipboard';
import { ChartsModule } from 'ng2-charts';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { AlertifyService } from './_services/alertify.service';
import { appRoutes } from './routes';
import { AuthGuard } from './_guards/auth.guard';
import { UserService } from './_services/user.service';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { AddLocationComponent } from './add-location/add-location.component';
import { LocationsListComponent } from './locations-list/locations-list.component';
import { LocationsListResolver } from './_resolvers/locations-list.resolver';
import { LocationService } from './_services/location.service';
import { LocationCardComponent } from './location-card/location-card.component';
import { LocationDetailComponent } from './location-detail/location-detail.component';
import { LocationDetailResolver } from './_resolvers/location-detail.resolver';
import { LocationDetailMovementsResolver } from './_resolvers/location-detail-movements.resolver';
import { SignalRService } from './_services/signal-r.service';
import { environment } from 'src/environments/environment';

export function tokenGetter() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberEditComponent,
      PhotoEditorComponent,
      AddLocationComponent,
      LocationsListComponent,
      LocationCardComponent,
      LocationDetailComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      BsDropdownModule.forRoot(),
      TabsModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      NgxGalleryModule,
      FileUploadModule,
      ClipboardModule,
      ChartsModule,
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: [environment.whitelistedDomain],
            blacklistedRoutes: [environment.blacklistedDomain]
         }
      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorProvider,
      AlertifyService,
      AuthGuard,
      UserService,
      MemberEditResolver,
      PreventUnsavedChanges,
      LocationsListResolver,
      LocationService,
      LocationDetailResolver,
      LocationDetailMovementsResolver,
      SignalRService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
