// Datenklasse Device als JS-Entsprechung für die C#-Models definieren
export default class Device {
    Id;
    Name;
    DeviceType;
    Manufacturer;
    SerialNumber;
    Accessories;
    Comments;
    Software = [];
}