import {Component, Inject, NgModule} from '@angular/core';
import {VehicleService} from "../services/vehicle.service";
import {Make, Model, Feature} from "../Interfaces/MakeInterfaces";
import {ToastyService} from "ng2-toasty";
import {ActivatedRoute, Router} from "@angular/router";
import {Observable} from "rxjs";
import {VehicleResource} from "../types/resources/vehicle-resources";
import {CommonModule} from "@angular/common";

@Component({
  selector: 'app-vehicles-list',
  templateUrl: './vehicles-list.component.html',
})
export class VehiclesListComponent {
  protected vehicles: VehicleResource[] = [];
  protected meta = {};

  constructor(
    private vehicleService: VehicleService,
  ) {
    this.getVehicles();
  }

  getVehicles() {
    this.vehicleService.getVehicles()
      .subscribe(l => {
        this.vehicles = l.data;
        this.meta = l.meta;
      }, e => {
        alert('list could not be fetched');
        console.log({e})
      })
  }
}
