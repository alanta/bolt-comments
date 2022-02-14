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
  refresh: () => void;
}

const Approvals : React.FC<ApprovalsProps> = (props:ApprovalsProps) =>  {

  const { approvalsService, removeItem, refresh } = {...props};
  const updateService = useUpdateCommentService();
  useEffect(() => {
      document.title = "Approvals - Bolt Comments"
  }, []);

  const approveComment = (event: React.MouseEvent<HTMLElement,MouseEvent>, comment : Comment) => {
    event.preventDefault();
    updateService.approveComment(comment.id).then(() =>{ removeItem(comment) })
    refresh();
  };

  const deleteComment = (event: React.MouseEvent<HTMLElement,MouseEvent>, comment : Comment) => {
    event.preventDefault();
    updateService.deleteComment(comment.id).then(() =>{ removeItem(comment) })
    refresh();
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
          <thead className="bg-primary text-white">
            <tr>
              <th scope="col" className="w-10">From</th>
              <th scope="col" className="w-auto">Comment</th>
              <th scope="col" className="w-10">Actions <i className="fas fa-sync" onClick={() => refresh()}></i></th>
            </tr>
          </thead>
          <tbody>
          {approvalsService.payload.map(commentsByKey => (
            <>
            <tr key={commentsByKey.key} className="bg-purple text-white">
              <th colSpan={5}><i className="fas fa-feather mr-4"></i> {commentsByKey.key}</th>
            </tr>
            {commentsByKey.comments.map( comment => (  
            <tr key={comment.id}>
              <td>
              <div className="row justify-content-center mb-2">
                <img className="rounded-circle shadow" src={comment.avatar} width={70} alt={comment.name} />
              </div>
              <div className="text-center">{comment.name}<br/>
                {comment.email}</div>
              </td>
              <td><h6 className="mb-2 font-weight-bold"><i className="far fa-calendar-alt"></i> {moment(comment.posted).calendar()}</h6><div className="comment-content" dangerouslySetInnerHTML={{__html: comment.content}}></div></td>
              <td className="text-nowrap">
                <button type="button" className="btn btn-sm btn-success btn-round mr-1" title="Approve" onClick={(e) => approveComment(e, comment)}><i className="fas fa-check"></i></button> 
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