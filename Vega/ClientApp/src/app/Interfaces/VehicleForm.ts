import {Make} from "./MakeInterfaces";

export interface VehicleForm {
  make: Make;
  model: Make;
  ContactName: string;
  ContactPhone: string;
  ContactEmail: string;
  VehicleFeatureIds: number[];
}
