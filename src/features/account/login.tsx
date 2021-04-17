export default function Login(props: any) {
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
                <h3 className="mb-4 text-center">Sign In</h3>
                <a type="button" className="btn btn-lg btn-info btn-round mb-1 shadow-sm" href="/.auth/login/aad"><i className="fab fa-microsoft"></i> Login with Microsoft</a><br/>
                <a type="button" className="btn btn-lg btn-info btn-round mb-1 shadow-sm" href="/.auth/login/github"><i className="fab fa-github"></i> Login with Github</a><br/>
                <a type="button" className="btn btn-lg btn-info btn-round mb-1 shadow-sm" href="/.auth/login/google"><i className="fab fa-google"></i> Login with Google</a><br/>
                <a type="button" className="btn btn-lg btn-info btn-round mb-1 shadow-sm" href="/.auth/login/twitter"><i className="fab fa-twitter"></i> Login with Twitter</a><br/>
                <a type="button" className="btn btn-lg btn-info btn-round mb-1 shadow-sm" href="/.auth/login/facebook"><i className="fab fa-facebook"></i> Login with Facebook</a>
            </div>
		</div>
	</div>
</div>
)}