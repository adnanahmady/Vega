import {Injectable} from "@angular/core";
import {Observable} from "rxjs";
import {Make} from "../Interfaces/MakeInterfaces";
import {HttpClient} from "@angular/common/http";

@Injectable()
export class MakeService {
  private baseUrl: string = 'https://localhost:5001/api/v1/makes';

  constructor(private http: HttpClient) {}

  getMakes(): Observable<Make[]> {
    return this.http.get<Make[]>(this.baseUrl);
  }
}
