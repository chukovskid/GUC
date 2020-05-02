import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept( // ova ke gi fati site errori od 400 i 500
    req: import('@angular/common/http').HttpRequest<any>, // ova gleda sto ke dojde
    next: import('@angular/common/http').HttpHandler // so ova sto pravime NAREDNO ?posle errorot
  ): import('rxjs').Observable<import('@angular/common/http').HttpEvent<any>> {
    return next.handle(req).pipe(
        catchError(error => { // ova eeror e nasiot HttpEror u console headers so go dobivame
            if (error.status === 401){
                return throwError(error.statusText);
            }
            if (error instanceof HttpErrorResponse ){ // ako e od 500 nagore i e EXEPTION
                const applicationError = error.headers.get('Application-Error');
                if (applicationError){
                    return throwError(applicationError);
                }
                const serveError = error.error;

                // dole e za (password incorest, too short) NOT MET VALIDATION REQ
                let modalStateErrors = '';
                if (serveError.errors && typeof serveError.errors === 'object'){
                    for (const key in serveError.errors){ // [KEY] moze da bide password ili acc itn..
                        if (serveError.errors[key]){
                            modalStateErrors += serveError.errors[key] + 'l\n';
                            // ^ ova Gore e za sekoj error od razlicen KEY da bide na posebna linija
                        }
                    }
                }
                return throwError(modalStateErrors || serveError || 'Server Erroe e '); // znaci ako nemam MODEL error so KEY..
            // ..togas daj Error (nivo pogore) ako ne e ni serverError pisi deka e "Server Error"
            }
        })
    );
  }
}

export const ErrorInterceptorProvider = {
 provide: HTTP_INTERCEPTORS,
 useClass: ErrorInterceptor, // koja class da ja koristi (OVAA)
 multi: true // deka ke imame Povekje od EDEN Error
};
