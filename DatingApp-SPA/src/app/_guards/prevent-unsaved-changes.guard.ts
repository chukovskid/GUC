import { Injectable } from '@angular/core';
import { CanActivate, Router, CanDeactivate } from '@angular/router';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChanges implements CanDeactivate<MemberEditComponent> {

  canDeactivate(component: MemberEditComponent){
    if (component.editForm.dirty){ // proveruvam dali componentot Edit dali e "dirty"
  return confirm('Are you sure you want to leave. the data will not be saved'); // ako e ..
    }
    return true; // ako ne e vrati true za da prodolzi CanDeactivate na nareden route
  }
  }

