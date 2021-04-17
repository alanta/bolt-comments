export default function Login(props: any) {
    return (
      
<div className="d-md-flex h-md-100 align-items-center">
	<div className="col-md-6 p-0 bg-indigo h-md-100">
		<div className="text-white d-md-flex align-items-center h-100 p-5 text-center justify-content-center">
			<div className="logoarea pt-5 pb-5">
				<p>
					<i className="fa fa-anchor fa-3x"></i>
				</p>
				<h1 className="mb-0 mt-3 display-4">Anchor</h1>
				<h5 className="mb-4 font-weight-light">Free Bootstrap UI Kit with <i className="fab fa-sass fa-2x text-cyan"></i></h5>
				<a className="btn btn-outline-white btn-lg btn-round" href="#" data-toggle="modal" data-target="#modal_newsletter">Download <a href="https://github.com/wowthemesnet/Anchor-Bootstrap-UI-Kit/archive/master.zip" className="downloadzip hidden"></a>
				</a>
			</div>
		</div>
	</div>
	<div className="col-md-6 p-0 bg-white h-md-100 loginarea">
		<div className="d-md-flex align-items-center h-md-100 p-5 justify-content-center">
			<form className="border rounded p-5">
				<h3 className="mb-4 text-center">Sign In</h3>
				<div className="form-group">
					<input type="email" className="form-control" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="E-mail" required={true}/>
				</div>
				<div className="form-group">
					<input type="password" className="form-control" id="exampleInputPassword1" placeholder="Password" required={true}/>
				</div>
				<div className="form-group form-check">
					<input type="checkbox" className="form-check-input" id="exampleCheck1"/>
					<label className="form-check-label small text-muted" htmlFor="exampleCheck1">Remember me</label>
				</div>
				<button type="submit" className="btn btn-success btn-round btn-block shadow-sm">Sign in</button>
				<small className="d-block mt-4 text-center"><a className="text-gray" href="#">Forgot your password?</a></small>
			</form>
		</div>
	</div>
</div>
)}