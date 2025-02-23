import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {VehicleService} from "../services/vehicle.service";
import {Make, Model, Feature} from "../Interfaces/MakeInterfaces";
import {concatWith} from "rxjs";

@Component({
  selector: 'app-form',
  templateUrl: './vehicle-form.component.html'
})
export class VehicleFormComponent {
  public makes: Make[] = [];
  public models: Model[] = [];
  public features: Feature[] = [];
  public selectedMake: null|Make = null;
  public vehicle: any = {};

  constructor(
    private vehicleService: VehicleService
    ) {
    this.getMakes();
    this.getVehicleFeatures();
  }

  getMakes(): void {
    this.vehicle.make = 0;
    this.vehicleService.getMakes()
      .subscribe(makes => this.makes = makes);
  }

  getVehicleFeatures(): void {
    this.vehicleService.getFeatures()
      .subscribe(features => this.features = features);
  }

  handleMakeChange(event: any) {
    const selected = event.target.value;
    const make = this.makes
      .filter(m => m.id == selected)[0] || null;
    if (make === null) {
      this.vehicle.make = 0;
      this.models = [];
      return;
    }
    this.vehicle.make = make.id;
    this.models = make?.models;
  }

  handleSubmit() {
    console.log("Form submitted")
  }
}
