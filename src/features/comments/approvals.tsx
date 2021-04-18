import Header, { UISize } from "components/header";
import React, { useEffect } from "react";
import moment from 'moment'
import { useApprovalsService } from 'services/comments.service';

const Approvals : React.FC<{}> = () =>  {

  const service = useApprovalsService();
  useEffect(() => {
      document.title = "Approvals - Bolt Comments"
  }, []);

  return (
    <>
    <Header size={UISize.Small}>
        <div className="col pt-4 pb-4">
            <h1 className="display-3">Approvals</h1>
        </div>
    </Header>
    <section className="pt-4 pb-5 aos-init aos-animate">
        <div className="container">
        {service.status === 'loading' && <div className="alert alert-purple" role="alert"><i className="fas fa-spinner"></i> Loading...</div>}
    {service.status === 'error' && (
        <div className="alert alert-danger" role="alert"><i className="fas fa-times"></i> Error, the backend moved to the dark side.</div>
      )}
    {service.status === 'loaded' && (!service.payload || service.payload.length === 0 ) &&
        <div className="alert alert-success" role="alert"><i className="fas fa-check"></i> No pending approvals found.</div> }
    {service.status === 'loaded' && (service.payload && service.payload.length > 0 ) &&
      
        <table className="table table-hover">
          <thead className="thead-dark">
            <tr>
              <th scope="col">Received</th>
              <th scope="col">Name</th>
              <th scope="col">Email</th>
              <th scope="col">Comment</th>
              <th scope="col">Actions</th>
            </tr>
          </thead>
          <tbody>
          {service.payload.map(comment => (
            <tr key={comment.id}>
              <th scope="row">{moment(comment.posted).calendar()}</th>
              <td>{comment.name}</td>
              <td>{comment.email}</td>
              <td>{comment.content}</td>
              <td><button type="button" className="btn btn-sm btn-success btn-round" title="Approve"><i className="fas fa-check"></i></button> <button type="button" className="btn btn-sm btn-danger btn-round"  title="Delete"><i className="far fa-trash-alt"></i></button></td>
            </tr>
          ))}
          </tbody>
        </table>
        }
        </div>
      </section>
    </>
  );
}



export default Approvals;