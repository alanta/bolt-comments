import Header, { UISize } from "components/header";
import React, { useEffect } from "react";
import moment from 'moment'
import useUpdateCommentService, { useCommentsService } from 'services/comments.service';
import { Comment } from 'models/comment';
import { useAuth } from "services/auth.service";

export interface CommentsProps 
{
  refresh: () => void;
}

const Comments : React.FC<CommentsProps> = (props: CommentsProps) =>  {
  const { refresh } = {...props};
  const {service, removeItem} = useCommentsService();
  const updateService = useUpdateCommentService();
  const auth = useAuth();
  
  useEffect(() => {
    document.title = "Comments - Bolt Comments"
}, []);

const rejectComment = (event: React.MouseEvent<HTMLElement,MouseEvent>, comment : Comment) => {
  event.preventDefault();
  updateService.rejectComment(comment.id).then(() =>{ removeItem(comment) })
  refresh();
};

  return (
    <>
    <Header size={UISize.Small}>
        <div className="col pt-4 pb-4">
            <h1 className="display-3">Comments</h1>
        </div>
    </Header>
    <section className="pt-4 pb-5 aos-init aos-animate">
        <div className="container">
        {service.status === 'loading' && <div className="alert alert-purple" role="alert"><i className="fas fa-spinner"></i> Loading...</div>}
    {service.status === 'error' && (
        <div className="alert alert-danger" role="alert"><i className="fas fa-spinner"></i> Error, the backend moved to the dark side.</div>
      )}
     {service.status === 'loaded' && (!service.payload || service.payload.length === 0 ) &&
        <div className="alert alert-danger" role="alert"><i className="fas fa-exclamation-circle"></i> No comments found.</div> }
    {service.status === 'loaded' && (service.payload && service.payload.length > 0 ) &&

        <table className="table table-hover">
        <thead className="bg-primary text-white">
          <tr>
            <th scope="col" className="w-10">From</th>
            <th scope="col" className="w-auto">Comment</th>
            <th scope="col" className="w-10">Actions</th>
          </tr>
        </thead>
          <tbody>
          {service.payload.map(commentsByKey => (
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
              <td><h6 className="mb-2 font-weight-bold"><i className="far fa-calendar-alt"></i> {moment(comment.posted).calendar()}</h6><div className="comment-content" dangerouslySetInnerHTML={{__html: comment.content}} ></div></td>
              <td className="text-nowrap">
              { auth.status === 'loaded' && auth.payload.isInAnyRole(['admin','approve']) && <button type="button" className="btn btn-sm btn-outline-primary btn-round" title="Reject" onClick={(e) => rejectComment(e, comment)}><i className="fas fa-times"></i></button> }
              </td>
            </tr>
            ))}
            </>
          ))}
          </tbody>
        </table>
        }
        </div>
      </section>
    </>
  );
}

export default Comments;