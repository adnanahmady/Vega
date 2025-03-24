import {Injectable} from "@angular/core";
import {Subject} from "rxjs";
import {
  HttpEvent,
  HttpRequest,
  HttpHandler,
  HttpInterceptor,
  HttpEventType
} from "@angular/common/http";
import {Observable} from "rxjs";
import {tap} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class ProgressService {
  uploadProgress: Subject<any> = new Subject();
}

@Injectable()
export class HttpProgressInterceptor implements HttpInterceptor {
  constructor(private progressService: ProgressService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap(event => {
        if (event.type === HttpEventType.UploadProgress) {
          this.progressService.uploadProgress.next(event);
        }
      })
    );
  }
}

