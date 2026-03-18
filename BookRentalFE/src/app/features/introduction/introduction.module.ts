import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { IntroductionComponent } from './introduction.component';

@NgModule({
  declarations: [
    IntroductionComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    IntroductionComponent
  ]
})
export class IntroductionModule { }
