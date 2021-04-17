import { useEffect, useState } from 'react';
import { Service } from 'services/service';

export class UserInfo{
    authenticated: boolean = false;
    clientPrincipal?: any;
}

export class clientPrincipal

{"clientPrincipal":{"userId":"f7afd2e4f3c9cb82edd3c283e6d48cb9","userRoles":["anonymous","authenticated"],"identityProvider":"aad","userDetails":"Marnix van Valen"}}

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
