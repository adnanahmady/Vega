export type IdNameType = {
  id: number,
  name: string,
}

export type VehicleResource = {
  id?: number,
  make: IdNameType;
  model: IdNameType;
  isRegistered: boolean;
  contact: {
    name: string;
    phone: string;
    email: string;
  };
  vehicleFeatures: IdNameType[];
}
