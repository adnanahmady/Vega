import {Injectable} from "@angular/core";
import {HttpClient, HttpEvent} from "@angular/common/http";
import {Observable} from "rxjs";
import {PhotoResource} from "../types/resources/photo-resource";

@Injectable()
export class PhotoService {
  private baseUrl: string = 'https://localhost:5001/api/v1/vehicles';

  constructor(private http: HttpClient) {
  }

  upload(vehicleId: number, photo: File) : Observable<HttpEvent<PhotoResource>> {
    var formData = new FormData();
    formData.append('file', photo);
    return this.http.post<PhotoResource>(
      this.baseUrl + `/${vehicleId}/photos`,
      formData,
      {
        reportProgress: true,
        observe: 'events'
      }
    );
  }

  getPhotos(vehicleId: number): Observable<PhotoResource[]> {
    return this.http.get<PhotoResource[]>(
      this.baseUrl + `/${vehicleId}/photos`,
    );
  }
}
