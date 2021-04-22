import React, { useEffect } from "react";

export default function Unauthorized(props: any) {
    useEffect(() => {
        document.title = "Unauthorized - Bolt Comments"
    }, []);

    return (
      
<div className="d-md-flex h-md-100 align-items-center">
	<div className="col-md-6 p-0 bg-indigo h-md-100">
		<div className="text-white d-md-flex align-items-center h-100 p-5 text-center justify-content-center">
			<div className="logoarea pt-5 pb-5">
				<p>
					<i className="fa fa-bolt fa-3x text-warning"></i>
				</p>
				<h1 className="mb-0 mt-3 display-4"><strong>Bolt</strong> Comments</h1>
				<h5 className="mb-4 font-weight-light">Easy self-hosted comments</h5>
			</div>
		</div>
	</div>
	<div className="col-md-6 p-0 bg-white h-md-100 loginarea">
		<div className="d-md-flex align-items-center h-md-100 p-5 justify-content-center">
            <div className="border rounded p-5">
                <h3 className="mb-4 text-center">Unauthorized</h3>
                <div className="alert alert-danger" role="alert"><i className="fas fa-lock"></i> Sorry, you're not allowed to access that page.</div>
            </div>
		</div>
	</div>
</div>
)}