export default function Footer(props:any) {
    return <>
        <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" viewBox="0 0 1440 126"  >
        <path className="bg-black" d="M685.6,38.8C418.7-11.1,170.2,9.9,0,30v96h1440V30C1252.7,52.2,1010,99.4,685.6,38.8z"/>
        </svg>
        <footer className="bg-black pb-5">
        <div className="container">
            <div className="row">
                <div className="col-12 col-md mr-4">
                    <i className="fas fa-copyright text-white"></i>
                    <small className="d-block mt-3 text-muted">© {new Date().getFullYear()} Alanta </small>
                </div>
                <div className="col-6 col-md">
                    <h5 className="mb-4 text-white">Features</h5>
                    <ul className="list-unstyled text-small">
                        <li><span className="text-muted">Self hosted on Azure</span></li>
                        <li><span className="text-muted">100% Serverless</span></li>
                        <li><span className="text-muted">No tracking</span></li>
                        <li><span className="text-muted">Jamstack proof</span></li>
                    </ul>
                </div>
                <div className="col-6 col-md">
                    <h5 className="mb-4 text-white">Resources</h5>
                    <ul className="list-unstyled text-small">
                        <li><a className="text-muted" href="https://github.com/alanta/bolt-comments">Github</a></li>
                        <li><a className="text-muted" href="https://github.com/alanta/bolt-comments">Geting started</a></li>
                        <li><a className="text-muted" href="https://github.com/alanta/bolt-comments/wiki">Docs</a></li>
                        <li><a className="text-muted" href="https://github.com/alanta/bolt-comments/issues">Report an issue</a></li>
                    </ul>
                </div>
                <div className="col-6 col-md">
                    <h5 className="mb-4 text-white">About</h5>
                    <ul className="list-unstyled text-small">
                        <li><a className="text-muted" href="https://github.com/alanta/bolt-comments">Team</a></li>
                        <li><a className="text-muted" href="https://github.com/alanta/bolt-comments">Contribute</a></li>
                        <li><a className="text-muted" href="https://github.com/alanta/bolt-comments">Donate</a></li>
                        
                    </ul>
                </div>
            </div>
        </div>
        </footer>
        </>
}