import React from "react";
import Header from "components/header";

export default function Home(props: any) {
  return (
    <>
      <Header>
            
                <div className="col pt-4 pb-4">
                    <h1 className="display-3">Bolt Comments</h1>
                    <h5 className="font-weight-light mb-4">Easy self-hosted comments for your blog on Azure functions <svg id="a2c88306-fa03-4e5b-b192-401f0b77808b" xmlns="http://www.w3.org/2000/svg" width="40" height="40" viewBox="0 0 18 18"><defs><linearGradient id="b403aca7-f387-4434-96b4-ae157edc835f" x1="-175.993" y1="-343.723" x2="-175.993" y2="-359.232" gradientTransform="translate(212.573 370.548) scale(1.156 1.029)" gradientUnits="userSpaceOnUse"><stop offset="0" stop-color="#fea11b" /><stop offset="0.284" stop-color="#fea51a" /><stop offset="0.547" stop-color="#feb018" /><stop offset="0.8" stop-color="#ffc314" /><stop offset="1" stop-color="#ffd70f" /></linearGradient></defs><title>Icon-compute-29</title><g><path d="M2.37,7.475H3.2a.267.267,0,0,1,.267.267v6.148a.533.533,0,0,1-.533.533H2.1a0,0,0,0,1,0,0V7.741a.267.267,0,0,1,.267-.267Z" transform="translate(12.507 16.705) rotate(134.919)" fill="#50e6ff" /><path d="M2.325,3.6h.833a.267.267,0,0,1,.267.267v6.583a0,0,0,0,1,0,0H2.591a.533.533,0,0,1-.533-.533V3.865A.267.267,0,0,1,2.325,3.6Z" transform="translate(5.759 0.114) rotate(44.919)" fill="#1490df" /></g><g><path d="M14.53,7.475h.833a.533.533,0,0,1,.533.533v6.148a.267.267,0,0,1-.267.267H14.8a.267.267,0,0,1-.267-.267V7.475a0,0,0,0,1,0,0Z" transform="translate(12.223 -7.555) rotate(45.081)" fill="#50e6ff" /><path d="M15.108,3.6h.833a0,0,0,0,1,0,0v6.583a.267.267,0,0,1-.267.267h-.833a.267.267,0,0,1-.267-.267V4.131a.533.533,0,0,1,.533-.533Z" transform="translate(31.022 1.222) rotate(135.081)" fill="#1490df" /></g><path d="M8.459,9.9H4.87a.193.193,0,0,1-.2-.181.166.166,0,0,1,.018-.075L8.991,1.13a.206.206,0,0,1,.186-.106h4.245a.193.193,0,0,1,.2.181.165.165,0,0,1-.035.1L8.534,7.966h4.928a.193.193,0,0,1,.2.181.176.176,0,0,1-.052.122L5.421,16.788c-.077.046-.624.5-.356-.189h0Z" fill="url(#b403aca7-f387-4434-96b4-ae157edc835f)" /></svg></h5>
                    <a href="https://github.com/alanta/bolt-comments" className="btn btn-lg btn-outline-white btn-round">Learn more</a>
                </div>
            
      </Header>
      <main className="container">
        <section className="pt-4 pb-5" data-aos="fade-up">
          <h3 className="h5 mb-4 font-weight-bold">Icons</h3>
          <p className="mb-5">
            Anchor UI Kit comes with latest Font Awesome (5.3.1), the web’s most
            popular icon set and toolkit.
          </p>
          <span className="inline-block mr-3">
            <i className="fa fa-heart text-danger"></i>
            <i className="fa fa-heart text-danger fa-2x"></i>
            <i className="fa fa-heart text-danger fa-3x"></i>
          </span>
          <span className="mr-3">
            <span className="iconbox iconmedium rounded-circle text-primary">
              <i className="fab fa-facebook-f"></i>
            </span>
            <span className="iconbox iconmedium rounded-circle text-info">
              <i className="fab fa-twitter"></i>
            </span>
            <span className="iconbox iconmedium rounded-circle text-danger">
              <i className="fab fa-google"></i>
            </span>
          </span>
          <span className="mr-3">
            <span className="iconbox iconsmall fill rounded-circle bg-primary text-white shadow border-0">
              <i className="fas fa-cart-arrow-down"></i>
            </span>
            <span className="iconbox iconmedium fill rounded-circle bg-warning text-white shadow border-0">
              <i className="fas fa-coffee"></i>
            </span>
            <span className="iconbox iconlarge fill rounded-circle bg-success text-white shadow border-0">
              <i className="fa fa-book-reader"></i>
            </span>
          </span>
        </section>
      </main>
    </>
  );
}
