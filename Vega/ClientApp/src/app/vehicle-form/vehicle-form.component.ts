import {Component, Inject} from '@angular/core';
import {VehicleService} from "../services/vehicle.service";
import {Make, Model, Feature} from "../Interfaces/MakeInterfaces";
import {ToastyService} from "ng2-toasty";
import {ActivatedRoute, Router} from "@angular/router";
import {Observable} from "rxjs";
import {FeatureService} from "../services/feature.service";
import {MakeService} from "../services/make.service";

interface Vehicle {
  id?: number,
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
  public selectedMake: null | Make = null;
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
    private route: ActivatedRoute,
    private router: Router,
    private vehicleService: VehicleService,
    private makeService: MakeService,
    private featureService: FeatureService
  ) {
    route.params.subscribe(p => {
      this.vehicle.id = +p['id'];
    })
    this.getVehicle();
    this.getMakes();
    this.getVehicleFeatures();
  }

  getVehicle() {
    if (!this.vehicle.id) {
      return;
    }
    this.vehicleService.getVehicle(this.vehicle.id)
      .subscribe(v => {
        this.vehicle.makeId = v.make.id;
        this.getMakes();
        this.vehicle.modelId = v.model.id;
        this.vehicle.isRegistered = v.isRegistered;
        this.vehicle.contact = v.contact;
        this.vehicle.featureIds = v.vehicleFeatures
          .map(f => f.id);
      }, e => {
        if (e.status == 404) {
          this.router.navigate(['/']);
        }
      })
  }

  getMakes(): void {
    if (!this.vehicle.id) {
      this.vehicle.makeId = 0;
    }
    this.makeService.getMakes()
      .subscribe(makes => this.makes = makes);

    if (this.vehicle.makeId) {
      const make = this.makes.filter(m => m.id === this.vehicle.makeId);
      this.models = make[0].models;
    }
  }

  getVehicleFeatures(): void {
    this.featureService.getFeatures()
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
    if ($event.target.checked) {
      this.vehicle.featureIds.push(featureId);
      return;
    }

    const features = this.vehicle.featureIds.filter((id: any) => id !== featureId);
    this.vehicle.featureIds = features
  }

  handleSubmit() {
    if (this.vehicle.id) {
      this.vehicleService.update(this.vehicle)
        .subscribe(
          x =>
          this.router.navigate(['/']),
          err => { alert("Vehicle creation failed"); }
        );
      return;
    }
    this.vehicleService.create(this.vehicle)
      .subscribe(
        x => this.router.navigate(['/']),
        err => { alert("Vehicle creation failed"); }
      );
  }

  handleDelete() {
    if (!this.vehicle.id) {
      return;
    }
    if (!confirm("Are you sure to delete this item?")) {
      return;
    }

    this.vehicleService.delete(this.vehicle.id)
      .subscribe(x => {
        this.router.navigate(['/']);
      });
  }
}
