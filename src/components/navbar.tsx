import React from "react";
import { Link } from "react-router-dom";
import { useAuth } from "services/auth.service";
import { Service } from "services/service";
import { CommentsByKey } from 'models/comment';

export interface NavbarProps {
    service: Service<CommentsByKey[]>
}

export default function Navbar(props:NavbarProps) {

    const { service } = props; // todo : don't fetch number of pending approvals if not authenticated
    const auth = useAuth();

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
            { auth.status === 'loaded' && auth.payload.isInAnyRole(['admin','approve']) &&
            <li className="nav-item">
                <Link className="nav-link" to="/approvals">Approvals {service.status === 'loaded' && service.payload.length > 0 && <span className="badge badge-info ml-2">{service.payload.reduce((sum: number, b: CommentsByKey) => sum + b.comments.length, 0)}</span>}</Link>
			</li>
            }
            { auth.status === 'loaded' && auth.payload.isInRole('admin') &&
            <li className="nav-item">
                <Link className="nav-link" to="/settings">Settings</Link>
			</li>
            }
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