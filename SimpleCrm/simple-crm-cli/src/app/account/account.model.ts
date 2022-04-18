export interface UserSummaryViewModel {
  id: string;
  name: string;
  emailAddress: string;
  jwtToken: string;
  roles: string[];
}


export interface MicrosoftOptions {
  client_id: string;
  scope: string;
  state: string;
}

export interface CredentialsViewModel {
  emailAddress: string;
  password: string;
}

export function anonymousUser(): UserSummaryViewModel {
  return {
    id: '',
    name: 'Anonymous',
    emailAddress: '',
    jwtToken: '',
    roles: [],
  };
}
