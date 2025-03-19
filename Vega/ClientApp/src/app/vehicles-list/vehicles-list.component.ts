import {Component, Inject, NgModule} from '@angular/core';
import {VehicleService} from "../services/vehicle.service";
import {Make, Model, Feature} from "../Interfaces/MakeInterfaces";
import {ToastyService} from "ng2-toasty";
import {ActivatedRoute, Router} from "@angular/router";
import {Observable} from "rxjs";
import {IdNameType, VehicleResource} from "../types/resources/vehicle-resources";
import {CommonModule} from "@angular/common";

@Component({
  selector: 'app-vehicles-list',
  templateUrl: './vehicles-list.component.html',
})
export class VehiclesListComponent {
  protected vehicles: VehicleResource[] = [];
  protected allVehicles: VehicleResource[] = [];
  protected meta = {};
  protected makes: IdNameType[] = [];
  protected filter: any = [];

  constructor(
    private vehicleService: VehicleService,
  ) {
    this.getVehicles();
    this.getMakes();
  }

  getMakes(): void {
    this.vehicleService.getMakes()
      .subscribe((m: Make[]) => this.makes = m);
  }

  getVehicles() {
    this.vehicleService.getVehicles()
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
    let vehicles: VehicleResource[] = this.allVehicles;

    if (this.filter.makeId)
      vehicles = vehicles.filter(v => +v.make.id === +this.filter.makeId);

    if (this.filter.modelId)
      vehicles = vehicles.filter(v => +v.model.id === +this.filter.modelId);

    this.vehicles = vehicles;
  }

  resetFilter() {
    this.filter = {};
    this.handleFilterChange();
  }
}
