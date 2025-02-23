import { Injectable } from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Feature, Make} from "../Interfaces/MakeInterfaces";
import {Observable} from "rxjs";

@Injectable()
export class VehicleService {
  private baseUrl: string = 'https://localhost:7177/';

  constructor(private http: HttpClient) {
  }

  getMakes(): Observable<Make[]> {
    return this.http.get<Make[]>(this.baseUrl + 'api/v1/makes');
  }

  getFeatures(): Observable<Feature[]> {
    return this.http.get<Feature[]>(this.baseUrl + 'api/v1/features');
  }
}
