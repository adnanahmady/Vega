import {Component, Inject, NgModule} from '@angular/core';
import {VehicleFilters, VehicleService} from "../services/vehicle.service";
import {Make, Model, Feature} from "../Interfaces/MakeInterfaces";
import {ToastyService} from "ng2-toasty";
import {ActivatedRoute, Router} from "@angular/router";
import {Observable} from "rxjs";
import {IdNameType, VehicleResource} from "../types/resources/vehicle-resources";
import {CommonModule} from "@angular/common";
import {MakeService} from "../services/make.service";

@Component({
  selector: 'app-vehicles-list',
  templateUrl: './vehicles-list.component.html',
})
export class VehiclesListComponent {
  protected vehicles: VehicleResource[] = [];
  protected allVehicles: VehicleResource[] = [];
  protected meta = {};
  protected makes: IdNameType[] = [];
  protected filter: VehicleFilters = {};

  constructor(
    private vehicleService: VehicleService,
    private makeService: MakeService
  ) {
    this.getVehicles();
    this.getMakes();
  }

  getMakes(): void {
    this.makeService.getMakes()
      .subscribe((m: Make[]) => this.makes = m);
  }

  getVehicles() {
    this.vehicleService.getVehicles(this.filter)
      .subscribe(l => {
        this.vehicles = l.data;
        this.allVehicles = l.data;
        this.meta = l.meta;
      }, e => {
        alert('list could not be fetched');
        console.log({e})
      })
  }

  handleFilterChange(): void {
    this.getVehicles();
  }

  resetFilter() {
    this.filter = {};
    this.handleFilterChange();
  }
}
