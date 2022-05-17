import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { FlightModule } from './flight/flight.module';
import { HttpClientModule } from '@angular/common/http';
import { FLIGHT_ROUTES } from './flight/flight.routes';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    HttpClientModule,
    RouterModule.forRoot([...FLIGHT_ROUTES]),
    FlightModule,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
