
export interface Feature {
  id: number;
  name: string;
}

export interface Model {
  id: number;
  name: string;
}

export interface Make {
  id: number;
  name: string;
  models: Model[];
}
