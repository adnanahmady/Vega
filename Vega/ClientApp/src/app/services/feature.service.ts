import {Injectable} from "@angular/core";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Feature} from "../Interfaces/MakeInterfaces";

@Injectable()
export class FeatureService {
  private baseUrl: string = 'https://localhost:5001/api/v1/features';

  constructor(private http: HttpClient) {}

  getFeatures(): Observable<Feature[]> {
    return this.http.get<Feature[]>(this.baseUrl);
  }
}
