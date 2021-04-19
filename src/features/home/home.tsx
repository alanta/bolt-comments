import React, { useEffect } from "react";
import Header from "components/header";

export default function Home(props: any) {
    useEffect(() => {
        document.title = "Bolt Comments"
    }, []);

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
          <h3 className="h5 mb-4 font-weight-bold">Manage your own comments</h3>
          <p className="mb-5">
            Bolt Comments is a simple service with a nice UI to manage the blog comments for your Jamstack site. This is an Azure Static Web App, but you can host your site any platform you like.
          </p>

          <div className="container pt-5 pb-5 mt-4 aos-init aos-animate" data-aos="fade-up">
            <div className="row gap-y">
                <div className="col-md-6 col-xl-4">
                    <div className="media">
                        <div className="iconbox iconmedium rounded-circle text-info mr-4">
                            <i className="fas fa-layer-group"></i>
                        </div>
                        <div className="media-body">
                            <h5>Easy to integrate</h5>
                            <p className="text-muted">
                                Simple API, no fuss.<br/> Works with any Jamstack site.
                            </p>
                        </div>
                    </div>
                </div>
                <div className="col-md-6 col-xl-4">
                    <div className="media">
                        <div className="iconbox iconmedium rounded-circle text-warning mr-4">
                            <i className="fas fa-shield-alt"></i>
                        </div>
                        <div className="media-body">
                            <h5>Privacy proof</h5>
                            <p className="text-muted">
                                No tracking, you own your data.
                            </p>
                        </div>
                    </div>
                </div>
                <div className="col-md-6 col-xl-4">
                    <div className="media">
                        <div className="iconbox iconmedium rounded-circle text-cyan mr-4">
                            <i className="fab fa-microsoft"></i>
                        </div>
                        <div className="media-body">
                            <h5>Runs on Azure</h5>
                            <p className="text-muted">
                                Cloud native app, running on a world-class cloud.
                            </p>
                        </div>
                    </div>
                </div>
                <div className="col-md-6 col-xl-4">
                    <div className="media">
                        <div className="iconbox iconmedium rounded-circle text-purple mr-4">
                            <i className="fab fa-react"></i>
                        </div>
                        <div className="media-body">
                            <h5>Easy management UI</h5>
                            <p className="text-muted">
                                Manage your comments with a few clicks on any device.
                            </p>
                        </div>
                    </div>
                </div>
                <div className="col-md-6 col-xl-4">
                    <div className="media">
                        <div className="iconbox iconmedium rounded-circle text-dark mr-4">
                            <i className="fab fa-github"></i>
                        </div>
                        <div className="media-body">
                            <h5>Open source</h5>
                            <p className="text-muted">
                                The code is up on Github. Fork it and deploy your own instance.
                            </p>
                        </div>
                    </div>
                </div>
                <div className="col-md-6 col-xl-4">
                    <div className="media">
                        <div className="iconbox iconmedium rounded-circle text-salmon mr-4">
                            <i className="fas fa-burn"></i>
                        </div>
                        <div className="media-body">
                            <h5>Support</h5>
                            <p className="text-muted">
                                Sorry, that's up to you.
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
          
        </section>
      </main>
    </>
  );
}


