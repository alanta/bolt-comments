import Header from "components/header";
import React from "react";
import moment from 'moment'
import { useApprovalsService } from 'services/comments.service';


const Approvals : React.FC<{}> = () =>  {

  const service = useApprovalsService();

  return (
    <>
    <Header>
        <div className="col pt-4 pb-4">
            <h1 className="display-3">Approvals</h1>
        </div>
    </Header>
    {service.status === 'loading' && <div>Loading...</div>}
    {service.status === 'error' && (
        <div>Error, the backend moved to the dark side.</div>
      )}
    {service.status === 'loaded' &&
      <section className="pt-4 pb-5 aos-init aos-animate">
        <div className="container">
        <h3 className="h5 mb-4 font-weight-bold">Table</h3>
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
        </div>
      </section>}
    </>
  );
}


export default Approvals;