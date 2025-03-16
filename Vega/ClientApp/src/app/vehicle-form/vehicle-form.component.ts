import { Component, Inject } from '@angular/core';
import {VehicleService} from "../services/vehicle.service";
import {Make, Model, Feature} from "../Interfaces/MakeInterfaces";
import {ToastyService} from "ng2-toasty";

interface Vehicle {
  makeId?: number;
  modelId?: number;
  isRegistered: boolean;
  contact: {
    name: string;
    phone: string;
    email: string;
  };
  featureIds: number[];
}

@Component({
  selector: 'app-form',
  templateUrl: './vehicle-form.component.html'
})
export class VehicleFormComponent {
  public makes: Make[] = [];
  public models: Model[] = [];
  public features: Feature[] = [];
  public selectedMake: null|Make = null;
  public vehicle: Vehicle = {
    isRegistered: false,
    featureIds: [],
    contact: {
      name: "",
      phone: "",
      email: ""
    }
  };

  constructor(
    private vehicleService: VehicleService,
    private toastyService: ToastyService
    ) {
    this.getMakes();
    this.getVehicleFeatures();
  }

  getMakes(): void {
    this.vehicle.makeId = 0;
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
      this.vehicle.makeId = 0;
      delete this.vehicle.modelId;
      this.models = [];
      return;
    }
    this.vehicle.makeId = make.id;
    delete this.vehicle.modelId;
    this.models = make?.models;
  }

  handleFeatureToggle(featureId: number, $event: any) {
    if ($event.target.checked) { this.vehicle.featureIds.push(featureId); return; }

    const features = this.vehicle.featureIds.filter((id: any) => id !== featureId);
    this.vehicle.featureIds = features
  }

  handleSubmit() {
    this.vehicleService.create(this.vehicle)
      .subscribe(
        x => console.log("Success", x),
        err => {
          alert("Vehicle creation failed");
        }
      );
  }
}
