import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AutoCompleteComponent } from './components/auto-complete/auto-complete.component';

const routes: Routes = [
  { path: 'autocomplete', component: AutoCompleteComponent }
];

@NgModule({ 
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
