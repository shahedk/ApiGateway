import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { ServiceComponent } from './service/service.component';
import { ApiComponent } from './api/api.component';
import { RoleComponent } from './role/role.component';
import { KeyComponent } from './key/key.component';
import { ListComponent } from './api/list/list.component';
import { FormComponent } from './api/form/form.component';
import { SysComponent } from './sys/sys.component';
import { AppenvComponent } from './appenv/appenv.component';

@NgModule({
  declarations: [
    AppComponent,
    ServiceComponent,
    ApiComponent,
    RoleComponent,
    KeyComponent,
    ListComponent,
    FormComponent,
    SysComponent,
    AppenvComponent
  ],
  imports: [
    BrowserModule,
    ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
