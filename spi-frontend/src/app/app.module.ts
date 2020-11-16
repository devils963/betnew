import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainContainerComponent } from './main-container/main-container.component';
import { HeaderComponent } from './mobirise-components/header/header.component';
import { ContentComponent } from './mobirise-components/content/content.component';
import { FooterComponent } from './mobirise-components/footer/footer.component';
import { PricingtablesComponent } from './mobirise-components/pricingtables/pricingtables.component';
import { HttpClientModule } from '@angular/common/http';
import { GraphQLModule } from './graphql.module';

@NgModule({
  declarations: [
    AppComponent,
    MainContainerComponent,
    HeaderComponent,
    ContentComponent,
    FooterComponent,
    PricingtablesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    GraphQLModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
