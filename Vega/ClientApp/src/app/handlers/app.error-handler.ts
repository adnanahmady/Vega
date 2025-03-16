import * as ts from 'typescript';
import {ErrorHandler, Inject, Injectable, NgZone} from "@angular/core";
import {error} from "@angular/compiler-cli/src/transformers/util";
import {ToastyService} from "ng2-toasty";

@Injectable()
export class AppErrorHandler implements ErrorHandler {
  constructor(
    private ngZone: NgZone,
    @Inject(ToastyService) private toastyService: ToastyService
  ) {}

  handleError(error: any): void {
    this.ngZone.run(() => {
      this.toastyService.error({
        title: "Error",
        msg: 'An unexpected error happened.',
        theme: 'default',
        showClose: true,
        timeout: 5000,
      })
    });
  }
}
