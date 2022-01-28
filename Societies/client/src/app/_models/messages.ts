import { Read } from "../_enums/read";

export interface Messages {
  message: string;
  Sent: Date;
  Recieved: Date;
  Read: Read;
}
