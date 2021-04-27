import { useEffect, useState } from 'react';
import { Service } from 'services/service';
import { Settings } from 'models/boltSettings';

export const useSettingsService = () => {
  const [result, setResult] = useState<Service<Settings>>({
    status: 'loading'
  });

  useEffect(() => {
    fetch('/api/settings')
    .then(async (response) => {
      if( response.ok ){
        var data = await response.text();
        return Promise.resolve( !!!data || data.length === 0 ? [] : JSON.parse(data)  )
      }
    })
      .then(data => {
        setResult({ status: 'loaded', payload: data });
      })
      .catch(error => setResult({ status: 'error', error }));
  }, []);

  return result;
};


const useUpdateSettings = () => {
    const [service, setService] = useState<Service<Settings>>({
      status: 'init'
    });
  
    const update = (settings: Settings) => {
      setService({ status: 'loading' });
  
      const headers = new Headers();
      headers.append('Content-Type', 'application/json; charset=utf-8');
  
      return new Promise((resolve, reject) => {
        fetch('/api/settings', {
          method: 'POST',
          body: JSON.stringify(settings),
          headers
        })
        .then(async (response) => {
          if( response.ok ){
            var data = await response.text();
            return Promise.resolve( !!!data || data.length === 0 ? [] : JSON.parse(data)  )
          }
        })
          .then(response => {
            setService({ status: 'loaded', payload: response });
            resolve(response);
          })
          .catch(error => {
            setService({ status: 'error', error });
            reject(error);
          });
      });
    };
  
    return {
      service,
      update
    };
  };
  
  export default useUpdateSettings;