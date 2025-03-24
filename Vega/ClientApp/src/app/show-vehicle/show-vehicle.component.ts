import {Component, ElementRef, ViewChild} from "@angular/core";
import { VehicleResource } from "../types/resources/vehicle-resources";
import { VehicleService } from "../services/vehicle.service";
import { ActivatedRoute, Router } from "@angular/router";
import {PhotoService} from "../services/photo.service";
import {PhotoResource} from "../types/resources/photo-resource";

@Component({
  selector: 'app-show-vehicle',
  templateUrl: './show-vehicle.component.html',
})
export class ShowVehicleComponent {
  @ViewChild('fileInput') protected fileInput!: ElementRef;
  protected photos: PhotoResource[] = [];
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
  ) {
    this.route.params.subscribe(p => {
      this.vehicleService.getVehicle(p['id'])
        .subscribe(v => this.vehicle = v);
      this.photoService.getPhotos(p['id'])
        .subscribe(photos => this.photos = photos)
    }, () => this.router.navigate(['/']));
  }

  uploadPhoto() {
    if (this.vehicle.id === undefined) {
      return
    }

    var first: File;
    ({files: [first]} = this.fileInput.nativeElement);
    this.photoService.upload(this.vehicle.id, first)
      .subscribe(photo => {
        this.photos.push(photo)
      })
  }
}
