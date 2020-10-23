export type InteractionMethod = 'phone' | 'email';

export interface Customer {
  customerId: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  emailAddress: string;
  statusCode: string;
  preferredContactMethod: InteractionMethod;
  lastContactDate: string; // ISO format date
}
