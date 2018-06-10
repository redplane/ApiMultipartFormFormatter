import {NgModule} from '@angular/core';
import {NavigationBarComponent} from './navigation-bar/navigation-bar';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {RouterModule} from '@angular/router';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule
  ],
  declarations: [
    NavigationBarComponent
  ],
  exports: [
    NavigationBarComponent
  ]
})

export class SharedModule {

}
