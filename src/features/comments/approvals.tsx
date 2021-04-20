import Header, { UISize } from "components/header";
import React, { useEffect } from "react";
import moment from 'moment'
import useUpdateCommentService from 'services/comments.service';
import { Service } from "services/service";
import { Comment, CommentsByKey } from 'models/comment';

export interface ApprovalsProps 
{
  approvalsService: Service<CommentsByKey[]>;
  removeItem: (comment: Comment) => void;
}

const Approvals : React.FC<ApprovalsProps> = (props:ApprovalsProps) =>  {

  const { approvalsService, removeItem } = {...props};
  const updateService = useUpdateCommentService();
  useEffect(() => {
      document.title = "Approvals - Bolt Comments"
  }, []);

  const approveComment = (event: React.MouseEvent<HTMLElement,MouseEvent>, comment : Comment) => {
    event.preventDefault();
    updateService.approveComment(comment.id).then(() =>{ removeItem(comment) })
  };

  const deleteComment = (event: React.MouseEvent<HTMLElement,MouseEvent>, comment : Comment) => {
    event.preventDefault();
    updateService.deleteComment(comment.id).then(() =>{ removeItem(comment) })
  };

  return (
    <>
    <Header size={UISize.Small}>
        <div className="col pt-4 pb-4">
            <h1 className="display-3">Approvals</h1>
        </div>
    </Header>
    <section className="pt-4 pb-5 aos-init aos-animate">
        <div className="container">
        {approvalsService.status === 'loading' && <div className="alert alert-purple" role="alert"><i className="fas fa-spinner"></i> Loading...</div>}
    {approvalsService.status === 'error' && (
        <div className="alert alert-danger" role="alert"><i className="fas fa-times"></i> Error, the backend moved to the dark side.</div>
      )}
    {approvalsService.status === 'loaded' && (!approvalsService.payload || approvalsService.payload.length === 0 ) &&
        <div className="alert alert-success" role="alert"><i className="fas fa-check"></i> No pending approvals found.</div> }
    {approvalsService.status === 'loaded' && (approvalsService.payload && approvalsService.payload.length > 0 ) &&
        <>
            {updateService.service.status === 'loading' && <div>Sending...</div>}
            {updateService.service.status === 'loaded' && <div>Done</div>}
            {updateService.service.status === 'error' && <div>Oops</div>}
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
          {approvalsService.payload.map(commentsByKey => (
            <>
            <tr key={commentsByKey.key}>
              <th colSpan={5}>{commentsByKey.key}</th>
            </tr>
            {commentsByKey.comments.map( comment => (  
            <tr key={comment.id}>
              <th scope="row">{moment(comment.posted).calendar()}</th>
              <td>{comment.name}</td>
              <td>{comment.email}</td>
              <td>{comment.content}</td>
              <td>
                <button type="button" className="btn btn-sm btn-success btn-round" title="Approve" onClick={(e) => approveComment(e, comment)}><i className="fas fa-check"></i></button> 
                <button type="button" className="btn btn-sm btn-danger btn-round"  title="Delete" onClick={(e) => deleteComment(e, comment)}><i className="far fa-trash-alt"></i></button></td>
            </tr>
            ))}
            </>
          ))}
          </tbody>
        </table>
        </>
        }
        </div>
      </section>
    </>
  );
}



export default Approvals;