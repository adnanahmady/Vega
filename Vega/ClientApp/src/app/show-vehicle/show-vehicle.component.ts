import {Component, ElementRef, NgZone, ViewChild} from "@angular/core";
import { VehicleResource } from "../types/resources/vehicle-resources";
import { VehicleService } from "../services/vehicle.service";
import { ActivatedRoute, Router } from "@angular/router";
import {PhotoService} from "../services/photo.service";
import {PhotoResource} from "../types/resources/photo-resource";
import { ProgressService } from "../services/progress.service";
import {HttpEvent, HttpEventType, HttpResponse} from "@angular/common/http";
import {isAuthenticated} from "../auth/auth";
import {AuthService} from "@auth0/auth0-angular";

@Component({
  selector: 'app-show-vehicle',
  templateUrl: './show-vehicle.component.html',
})
export class ShowVehicleComponent {
  @ViewChild('fileInput') protected fileInput!: ElementRef;
  protected photos: PhotoResource[] = [];
  protected progress: number = 0;
  protected vehicle: VehicleResource = {
    make: {
      id: 0,
      name: ''
    },
    model: {
      id: 0,
      name: ''
    },
    isRegistered: false,
    contact: {
      name: '',
      phone: '',
      email: ''
    },
    vehicleFeatures: []
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private vehicleService: VehicleService,
    private photoService: PhotoService,
    private progressService: ProgressService,
    protected auth: AuthService
  ) {
    this.route.params.subscribe(p => {
      this.vehicleService.getVehicle(p['id'])
        .subscribe(v => this.vehicle = v);
      this.photoService.getPhotos(p['id'])
        .subscribe(photos => this.photos = photos)
    }, () => this.router.navigate(['/']));

    this.progressService.uploadProgress.subscribe(p => {
      this.progress = Math.round(100 * p.loaded / (p.total ?? 0));
    });
  }

  uploadPhoto() {
    if (this.vehicle.id === undefined) return;

    var file: File;
    ({files: [file]} = this.fileInput.nativeElement);
    this.fileInput.nativeElement.value = '';
    this.photoService.upload(this.vehicle.id, file)
      .subscribe((event: HttpEvent<PhotoResource>) => {
        if (event instanceof HttpResponse) {
          this.photos.push(event.body as PhotoResource);
        }
      })
  }

  protected readonly isAuthenticated = isAuthenticated;
}
