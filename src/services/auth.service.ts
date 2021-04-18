import { useEffect, useState } from 'react';
import { Service } from 'services/service';

export class UserInfo{
    authenticated: boolean = false;
    clientPrincipal?: clientPrincipal;
}

export interface clientPrincipal
{
  userId: string;
  userRoles?: string[];
  identityProvider:string;
  userDetails:string;
}

export const useAuthentication = () => {
  const [result, setResult] = useState<Service<UserInfo>>({
    status: 'loading'
  });

  useEffect(() => {
    fetch('/.auth/me')
      .then(response => response.json())
      .then(response => setResult(
        { 
          status: 'loaded', 
          payload: { 
            authenticated : !!response?.clientPrincipal,
            clientPrincipal: response.clientPrincipal
          }
        }))
      .catch(error => setResult({ status: 'error', error }));
  }, []);

  return result;
};
