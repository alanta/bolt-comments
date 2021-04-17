import { useEffect, useState } from 'react';
import { Service } from 'services/service';
import { Comment } from 'models/comment';

export const useCommentsService = () => {
  const [result, setResult] = useState<Service<Comment[]>>({
    status: 'loading'
  });

  useEffect(() => {
    fetch('/api/comment/approved')
      .then(response => response.json())
      .then(response => setResult({ status: 'loaded', payload: response }))
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
      .then(response => response.json())
      .then(response => setResult({ status: 'loaded', payload: response }))
      .catch(error => setResult({ status: 'error', error }));
  }, []);

  return result;
};