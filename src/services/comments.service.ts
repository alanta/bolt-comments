import { useEffect, useState } from 'react';
import { Service } from 'services/service';
import { Comment } from 'models/comment';

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

  const removeItem = (id:string) =>{
    if(result.status === 'loaded'){
      
      result.payload.forEach( (item, index) => {
        if(item.id === id) result.payload.splice(index,1);
      });

      setResult(prevStatus => ({
        ...prevStatus, payload: result.payload
      }));
    }
  }

  return { service : result, removeItem };
};

const useUpdateCommentService = () => {
  const [service, setService] = useState<Service<string>>({
    status: 'init'
  });

  const approveComment = (id: string) => {
    setService({ status: 'loading' });

    const headers = new Headers();
    headers.append('Content-Type', 'application/json; charset=utf-8');

    return new Promise((resolve, reject) => {
      fetch('/api/comment/approve/'+id, {
        method: 'POST',
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

  const deleteComment = (id: string) => {
    setService({ status: 'loading' });

    const headers = new Headers();
    headers.append('Content-Type', 'application/json; charset=utf-8');

    return new Promise((resolve, reject) => {
      fetch('/api/comment/'+id, {
        method: 'DELETE',
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
    approveComment,
    deleteComment
  };
};

export default useUpdateCommentService;