import { Injectable } from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {VehicleResource} from "../types/resources/vehicle-resources";

@Injectable()
export class VehicleService {
  private baseUrl: string = 'https://localhost:5001/api/v1/vehicles';

  constructor(private http: HttpClient) {
  }

  create(vehicle: any): Observable<VehicleResource> {
    vehicle.isRegistered = !!vehicle.isRegistered;

    return this.http.post<VehicleResource>(
      this.baseUrl,
      vehicle
    );
  }

  update(vehicle: any): Observable<VehicleResource> {
    vehicle.isRegistered = !!vehicle.isRegistered;

    return this.http.put<VehicleResource>(
      this.baseUrl + '/' + vehicle.id,
      vehicle
    );
  }

  delete(id: number): Observable<Object>
  {
    return this.http.delete(this.baseUrl + '/' + id);
  }

  getVehicle(id: number): Observable<VehicleResource> {
    return this.http.get<VehicleResource>(
      this.baseUrl + '/' + id
    );
  }

  getVehicles(filter: VehicleFilters): Observable<{ data: VehicleResource[], meta: object}> {
    return this.http.get<{ data: VehicleResource[], meta: object}>(
      this.baseUrl,
      {params: {...filter}}
    );
  }
}

export interface VehicleFilters {
  makeId?: number,
  modelId?: number,
  sortBy?: string,
  sortDirection?: string;
}
