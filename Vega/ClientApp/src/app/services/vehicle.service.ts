import { Injectable } from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Feature, Make} from "../Interfaces/MakeInterfaces";
import {Observable} from "rxjs";
import {VehicleResource} from "../types/resources/vehicle-resources";

@Injectable()
export class VehicleService {
  private baseUrl: string = 'https://localhost:5001/';

  constructor(private http: HttpClient) {
  }

  getMakes(): Observable<Make[]> {
    return this.http.get<Make[]>(this.baseUrl + 'api/v1/makes');
  }

  getFeatures(): Observable<Feature[]> {
    return this.http.get<Feature[]>(this.baseUrl + 'api/v1/features');
  }

  create(vehicle: any): Observable<VehicleResource> {
    vehicle.isRegistered = !!vehicle.isRegistered;

    return this.http.post<VehicleResource>(
      this.baseUrl + 'api/v1/vehicles',
      vehicle
    );
  }

  update(vehicle: any): Observable<VehicleResource> {
    vehicle.isRegistered = !!vehicle.isRegistered;

    return this.http.put<VehicleResource>(
      this.baseUrl + 'api/v1/vehicles/' + vehicle.id,
      vehicle
    );
  }

  delete(id: number): Observable<Object>
  {
    return this.http.delete('api/v1/vehicles/' + id);
  }

  getVehicle(id: number): Observable<VehicleResource> {
    return this.http.get<VehicleResource>(
      this.baseUrl + 'api/v1/vehicles/' + id
    );
  }
}
