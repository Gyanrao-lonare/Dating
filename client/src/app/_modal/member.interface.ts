import { Photo } from "./photo.interface";
export interface member {
    id: number;
    userName: string;
    age: number;
    konownAs?: any;
    photoUrl: string;
    createdDate: Date;
    lastActive: Date;
    gender: string;
    introduction: string;
    lookingFor: string;
    intrests?: any;
    city: string;
    country: string;
    photos: Photo[];
    status:boolean;
}
