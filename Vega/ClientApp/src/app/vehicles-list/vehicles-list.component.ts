import {Component, Inject, NgModule} from '@angular/core';
import {VehicleFilters, VehicleService} from "../services/vehicle.service";
import {Make, Model, Feature} from "../Interfaces/MakeInterfaces";
import {IdNameType, VehicleResource} from "../types/resources/vehicle-resources";
import {MakeService} from "../services/make.service";
import { AuthService } from '@auth0/auth0-angular';
import {isAuthenticated} from "../auth/auth";
import { Router } from '@angular/router';

@Component({
  selector: 'app-vehicles-list',
  templateUrl: './vehicles-list.component.html',
})
export class VehiclesListComponent {
  protected vehicles: VehicleResource[] = [];
  protected allVehicles: VehicleResource[] = [];
  protected meta: any = {};
  protected makes: IdNameType[] = [];
  protected query: VehicleFilters = {};
  protected columns: {
    title: string,
    key: string,
    isSortable?: boolean | undefined
  }[] = [
    { title: 'Id', key: 'id' },
    { title: 'Contact Name', key: 'contactName', isSortable: true },
    { title: 'Make', key: 'make', isSortable: true },
    { title: 'Model', key: 'model', isSortable: true },
  ];
  protected page: {
    totalItems: number,
    pageSize: number,
    currentPage: number
  } = {
    totalItems: 0,
    pageSize: 3,
    currentPage: 1
  };
  protected resetTrigger: number = 0;

  constructor(
    private vehicleService: VehicleService,
    private makeService: MakeService,
    protected auth: AuthService,
  ) {
    this.getVehicles();
    this.getMakes();
  }

  getMakes(): void {
    this.makeService.getMakes()
      .subscribe((m: Make[]) => this.makes = m);
  }

  getVehicles() {
    this.query.pageNumber = this.page.currentPage;
    this.query.pageSize = this.page.pageSize;
    this.vehicleService.getVehicles(this.query)
      .subscribe(l => {
        this.vehicles = l.data;
        this.allVehicles = l.data;
        this.meta = l.meta;
        this.page.totalItems = l.meta.pagination.totalRecords;
      }, e => {
        console.error({e})
        alert('list could not be fetched');
      })
  }

  handleFilterChange(): void {
    this.page.currentPage = 1;
    this.resetTrigger++;
    this.getVehicles();
  }

  resetFilter() {
    this.resetTrigger++;
    this.query = {};
    this.handleFilterChange();
  }

  sortBy(columnName: string): void {
    if (this.query.sortBy === columnName) {
      this.query.sortDirection = this.query.sortDirection === 'asc' ? 'desc' : 'asc';
    } else {
      this.query.sortBy = columnName;
      this.query.sortDirection = 'desc';
    }

    this.getVehicles();
  }

  handlePageChanged(page: number): void {
    this.page.currentPage = page;
    this.getVehicles();
  }

  protected readonly isAuthenticated = isAuthenticated;
}
