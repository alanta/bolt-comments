import Header from "components/header";
import React from "react";

export default function Comments(props: any) {
  return (
    <>
    <Header>
        <div className="col pt-4 pb-4">
            <h1 className="display-3">Comments</h1>
        </div>
    </Header>
      <section className="pt-4 pb-5 aos-init aos-animate">
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
            <tr id="1">
              <th scope="row">1</th>
              <td>Mark</td>
              <td>Otto</td>
              <td>@mdo</td>
              <td><button type="button" className="btn btn-sm btn-outline-primary btn-round" title="Reject"><i className="fas fa-times"></i></button></td>
            </tr>
            <tr id="2">
              <th scope="row">2</th>
              <td>Jacob</td>
              <td>Thornton</td>
              <td>@fat</td>
              <td><button type="button" className="btn btn-sm btn-outline-primary btn-round" title="Reject"><i className="fas fa-times"></i></button></td>
            </tr>
            <tr id="3">
              <th scope="row">3</th>
              <td>Larry</td>
              <td>the Bird</td>
              <td>@twitter</td>
              <td><button type="button" className="btn btn-sm btn-outline-primary btn-round" title="Reject"><i className="fas fa-times"></i></button></td>
            </tr>
          </tbody>
        </table>
      </section>
    </>
  );
}
