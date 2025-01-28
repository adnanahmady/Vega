import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html'
})
export class FormComponent {
  public makes: Make[] = [];
  public features: VehicleFeatures[] = [];
  private http: HttpClient;
  private baseUrl: string = 'https://localhost:7177/';
  public selectedMake: null|Make = null;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.getMakes();
    this.getVehicleFeatures();
  }

  getMakes(): void {
    this.http
      .get<Make[]>(this.baseUrl + 'api/v1/makes')
      .subscribe(
        result => this.makes = result,
        error => console.error(error)
      );
  }

  getVehicleFeatures(): void {
    this.http
      .get<VehicleFeatures[]>(this.baseUrl + 'api/v1/features')
      .subscribe(
        result => this.features = result,
        error => console.error(error)
      );
  }

  makeSelected(event: any) {
    const selected = event.target.value;
    this.selectedMake = this.makes
      .filter(m => m.id == selected)[0] || null;
    console.log('here', { event: selected, make: this.selectedMake })
  }
}

interface VehicleFeatures {
  id: number;
  name: string;
}

interface Model {
  id: number;
  name: string;
}

interface Make {
  id: number;
  name: string;
  models: Model[];
}
