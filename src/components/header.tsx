import './header.css'

export default function Header(props:any) {
    return <>
<header>
    <div className="jumbotron jumbotron-lg jumbotron-fluid mb-0 pb-3 bg-primary position-relative">
        <div className="container-fluid text-white h-100">
            <div className="d-lg-flex align-items-center justify-content-between text-center pl-lg-5">
            { props.children }
            </div>
        </div>
    </div>
    <svg className="wave" version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" viewBox="0 0 1440 126">
    <path className="bg-primary" d="M685.6,38.8C418.7-11.1,170.2,9.9,0,30v96h1440V30C1252.7,52.2,1010,99.4,685.6,38.8z"/>
    </svg>
</header>
</>
}