import { useEffect, useState } from 'react';
import { Service } from 'services/service';
import { Comment } from 'models/comment';
import { resolve } from 'q';

export const useCommentsService = () => {
  const [result, setResult] = useState<Service<Comment[]>>({
    status: 'loading'
  });

  useEffect(() => {
    fetch('/api/comment/approved')
    .then(async (response) => {
      if( response.ok ){
        var data = await response.text();
        return Promise.resolve( !!!data || data.length === 0 ? [] : JSON.parse(data)  )
      }
    })
      .then(data => setResult({ status: 'loaded', payload: data }))
      .catch(error => setResult({ status: 'error', error }));
  }, []);

  return result;
};

export const useApprovalsService = () => {
  const [result, setResult] = useState<Service<Comment[]>>({
    status: 'loading'
  });

  useEffect(() => {
    fetch('/api/comment/approvals')
      .then(async (response) => {
        if( response.ok ){
          var data = await response.text();
          return Promise.resolve( !!!data || data.length === 0 ? [] : JSON.parse(data)  )
        }
      })
      .then(data => setResult({ status: 'loaded', payload: data }))
      .catch(error => setResult({ status: 'error', error }));
  }, []);

  return result;
};
