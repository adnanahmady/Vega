import { BrowserModule } from '@angular/platform-browser';
import {APP_INITIALIZER, ErrorHandler, NgModule} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { VehicleFormComponent } from "./vehicle-form/vehicle-form.component";
import {VehicleService} from "./services/vehicle.service";
import {ToastyModule, ToastyService} from "ng2-toasty";
import {AppErrorHandler} from "./handlers/app.error-handler";
import {VehiclesListComponent} from "./vehicles-list/vehicles-list.component";
import {FeatureService} from "./services/feature.service";
import {MakeService} from "./services/make.service";
import { PaginationComponent } from './shared/pagination/pagination.component';
import { ShowVehicleComponent } from './show-vehicle/show-vehicle.component';
import {PhotoService} from "./services/photo.service";
import { HttpProgressInterceptor, ProgressService } from './services/progress.service';
import {AuthGuard, AuthModule} from "@auth0/auth0-angular";
import {AuthButtonComponent} from "./auth/auth-button.component";
import {UserProfileComponent} from "./auth/user-profile.component";
import { AuthInterceptor } from './services/auth.interceptor';
import { environment } from '../environments/environment';
import { AuthInitService, InitializeAuth } from './services/auth-init.service';

@NgModule({
  bootstrap: [AppComponent],
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    VehicleFormComponent,
    VehiclesListComponent,
    PaginationComponent,
    ShowVehicleComponent,
    AuthButtonComponent,
    UserProfileComponent
  ],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    ToastyModule.forRoot(),
    RouterModule.forRoot([
      {path: '', redirectTo: '/vehicles', pathMatch: 'full'},
      {path: 'counter', component: CounterComponent},
      {path: 'fetch-data', component: FetchDataComponent},
      {path: 'vehicles', component: VehiclesListComponent},
      {path: 'vehicles/:id/show', component: ShowVehicleComponent},
      {path: 'vehicles/new', component: VehicleFormComponent, canActivate: [AuthGuard]},
      {path: 'vehicles/:id/edit', component: VehicleFormComponent, canActivate: [AuthGuard]},
    ]),
    AuthModule.forRoot({
      domain: environment.auth0.domain,
      clientId: environment.auth0.clientId,
      redirect_uri: environment.auth0.redirectUri,
      audience: environment.auth0.audience,
    }),
  ],
  providers: [
    {provide: ErrorHandler, useClass: AppErrorHandler},
    ProgressService,
    {provide: HTTP_INTERCEPTORS, useClass: HttpProgressInterceptor, multi: true},
    PhotoService,
    VehicleService,
    MakeService,
    FeatureService,
    ToastyService,
    AuthInitService,
    {
      provide: APP_INITIALIZER,
      useFactory: InitializeAuth,
      deps: [AuthInitService],
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    },
  ]
})
export class AppModule { }
