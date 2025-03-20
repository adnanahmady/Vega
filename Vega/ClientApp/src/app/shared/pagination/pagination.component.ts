import { Component, Input, Output, EventEmitter, OnChanges } from '@angular/core';

@Component({
  selector: 'pagination',
  template: `
    <nav *ngIf="totalItems > pageSize">
      <ul class="pagination">
        <li class="page-item" [class.disabled]="currentPage === 1">
          <a class="page-link" (click)="previous()" aria-label="Previous">
            <span aria-hidden="true">&laquo;</span>
          </a>
        </li>
        <li
          *ngFor="let page of pages"
          class="page-item"
          [class.active]="currentPage === page">
          <a class="page-link" (click)="changePage(page)">{{ page }}</a>
        </li>
        <li class="page-item" [class.disabled]="currentPage === pages.length">
          <a class="page-link" (click)="next()" aria-label="Next">
            <span aria-hidden="true">&raquo;</span>
          </a>
        </li>
      </ul>
    </nav>
  `
})
export class PaginationComponent implements OnChanges {
  @Input('total-items') totalItems: number = 0;
  @Input('page-size') pageSize = 10;
  @Input('reset') reset: number = 0;
  @Output('page-changed') pageChanged = new EventEmitter();
  pages: number[] = [];
  currentPage = 1;

  ngOnChanges() {
    const pagesCount = Math.ceil(this.totalItems / this.pageSize);
    this.pages = [];
    for (let i = 1; i <= pagesCount; i++)
      this.pages.push(i);

    if (this.reset) {
      this.currentPage = 1;
      this.pageChanged.emit(this.currentPage);
    }
  }

  changePage(page: number) {
    this.currentPage = page;
    this.pageChanged.emit(page);
  }

  previous() {
    if (this.currentPage === 1)
      return;

    this.currentPage--;
    this.pageChanged.emit(this.currentPage);
  }

  next() {
    if (this.currentPage === this.pages.length)
      return;

    this.currentPage++;
    this.pageChanged.emit(this.currentPage);
  }
}

