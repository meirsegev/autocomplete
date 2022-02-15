import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AutoCompleteComponent } from './components/auto-complete/auto-complete.component';
import { AutoCompleteService } from './services/auto-complete.service';

@NgModule({
  declarations: [
    AppComponent,
    AutoCompleteComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [AutoCompleteService],
  bootstrap: [AppComponent]
})
export class AppModule { }
