export interface VehicleForm {
  id?: number,
  makeId?: number;
  modelId?: number;
  isRegistered: boolean;
  contact: {
    name: string;
    phone: string;
    email: string;
  };
  featureIds: number[];
}
