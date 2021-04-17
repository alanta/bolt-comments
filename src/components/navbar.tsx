import React from "react";
import { Link } from "react-router-dom";

export default function Navbar(props:any) {
    return<>
<nav className="topnav navbar navbar-expand-lg navbar-dark bg-primary fixed-top">
<div className="container-fluid">
    <Link className="navbar-brand" to="/"><i className="fas fa-bolt mr-2 text-warning"></i><strong>Bolt</strong> Comments</Link>
	<button className="navbar-toggler collapsed" type="button" data-toggle="collapse" data-target="#navbarColor02" aria-controls="navbarColor02" aria-expanded="false" aria-label="Toggle navigation">
	<span className="navbar-toggler-icon"></span>
	</button>
	<div className="navbar-collapse collapse" id="navbarColor02">
		<ul className="navbar-nav mr-auto d-flex align-items-center">
			<li className="nav-item">
                <Link className="nav-link" to="/comments">Comments</Link>
			</li>
            <li className="nav-item">
                <Link className="nav-link" to="/approvals">Approvals</Link>
			</li>
			{/*<li className="nav-item dropdown">
			    <a className="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Examples </a>
                <div className="dropdown-menu" aria-labelledby="navbarDropdown">
                    <a className="dropdown-item" href="./landing.html">Home Landing</a>
                    <a className="dropdown-item" href="./login.html">User Login</a>
                    <a className="dropdown-item" href="./blog.html">Blog Index</a>
                    <a className="dropdown-item" href="./page.html">Sample Page</a>
                </div>
            </li>
			<li className="nav-item">
			    <a className="nav-link" href="https://github.com/alanta/bolt-comments">Docs</a>
			</li>*/}
		</ul>
		<ul className="navbar-nav ml-auto d-flex align-items-center">
			<li className="nav-item">
			<span className="nav-link">
			<Link className="btn btn-info btn-round shadow-sm" to="/login" ><i className="fas fa-arrow-alt-circle-right"></i> Login</Link>
			</span>
			</li>
		</ul>
	</div>
</div>
</nav>
</>
}