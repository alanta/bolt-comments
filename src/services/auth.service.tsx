import React, { useEffect, useState, useContext, createContext } from 'react';
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

const authContext = createContext<Service<UserInfo>>({status: 'loading'});

export const useAuth = () => {
  return useContext(authContext);
};

interface ProvideAuthProps{
  children?: React.ReactNode
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


export const ProvideAuth: React.FC<ProvideAuthProps> = (props) => {
  const auth = useAuthentication();
  
  return (<authContext.Provider value={auth}>{props.children}</authContext.Provider>);
}