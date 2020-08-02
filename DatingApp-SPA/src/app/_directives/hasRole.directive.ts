import {
  Directive,
  Input,
  OnInit,
  ViewContainerRef,
  TemplateRef,
} from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Directive({
  selector: '[appHasRole]',
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[]; // Lista od Roles ke pratam tuka
  isVisible = false;
  constructor(
    private viewContainerRef: ViewContainerRef,
    private templateRef: TemplateRef<any>, // html Elementot kade e vneseno appHasRole
    private authService: AuthService
  ) {}

  ngOnInit() {
    const userRoles = this.authService.decodedToken.roles as Array<string>;
    // ako nema roles, clear the viewContainer
    if (!userRoles) {
      this.viewContainerRef.clear();
    }

    // if The User Has the role isti so appHasRole, pokazi gi site
    if (this.authService.roleMatch(this.appHasRole)) {
      if (!this.isVisible) {
        // ako e false
        this.isVisible = true;
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      } else{
      this.isVisible = false;
      this.viewContainerRef.clear();
      }
    }
  }
}
