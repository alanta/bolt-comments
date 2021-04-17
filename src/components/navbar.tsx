import React from "react";
import { Link } from "react-router-dom";
import { useAuthentication } from "services/auth.service";
import { useApprovalsService } from "services/comments.service";

export default function Navbar(props:any) {

    const service = useApprovalsService();
    const auth = useAuthentication();

    return<>
<nav className="topnav navbar navbar-expand-lg navbar-dark bg-primary fixed-top">
<div className="container-fluid">
    <Link className="navbar-brand" to="/"><i className="fas fa-bolt mr-2 text-warning"></i><strong>Bolt</strong> Comments</Link>
	<button className="navbar-toggler collapsed" type="button" data-toggle="collapse" data-target="#navbarColor02" aria-controls="navbarColor02" aria-expanded="false" aria-label="Toggle navigation">
	<span className="navbar-toggler-icon"></span>
	</button>
	<div className="navbar-collapse collapse" id="navbarColor02">
        { auth.status === 'loaded' && auth.payload.authenticated &&
		<ul className="navbar-nav mr-auto d-flex align-items-center">
			<li className="nav-item">
                <Link className="nav-link" to="/comments">Comments</Link>
			</li>
            <li className="nav-item">
                <Link className="nav-link" to="/approvals">Approvals {service.status === 'loaded' && <span className="badge badge-info ml-2">{service.payload.length}</span>}</Link>
			</li>
		</ul>
        }
		<ul className="navbar-nav ml-auto d-flex align-items-center">
			<li className="nav-item">
			<span className="nav-link">
            { auth.status === 'loaded' && !auth.payload.authenticated  && <Link className="btn btn-info btn-round shadow-sm" to="/login" ><i className="fas fa-arrow-alt-circle-right"></i> Login</Link> }
            { auth.status === 'loaded' && auth.payload.authenticated  && <a className="btn btn-info btn-round shadow-sm" href="/.auth/logout" ><i className="fas fa-times-circle"></i> Logout</a> }
			</span>
			</li>
		</ul>
	</div>
</div>
</nav>
</>
}