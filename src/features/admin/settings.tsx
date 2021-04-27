import React, { useEffect } from "react";
import Header, { UISize } from "components/header";
import { useForm } from "react-hook-form";
import useUpdateSettings, { useSettingsService } from "services/settings.service";

type Inputs = {
  apiKey?: string,
  webHookNewComment?: string,
  webHookCommentPublished?: string,
};


export default function Settings(props: any) {
    const { register, handleSubmit, setValue, formState: { errors } } = useForm<Inputs>();
    const service = useSettingsService();
    const updateService = useUpdateSettings();
    const onSubmit = (data:Inputs) => updateService.update(data);

    const validateUrl = (value:string|undefined) => {
      if( value === undefined || value === '' )
        return true;

      try {
        const url = new URL(value);
        return url.protocol === 'https:';
      } catch (TypeError) {
        return false;
      }
    }
    
    useEffect(() => {
        document.title = "Settings - Bolt Comments"
    }, []);

    useEffect(
      () => {
        if( service.status === 'loaded'){
          setValue("apiKey", service.payload.apiKey);
          setValue("webHookNewComment", service.payload.webHookNewComment);
          setValue("webHookCommentPublished", service.payload.webHookCommentPublished);
        }
      },
      [service,setValue]
    )

  return (
    <>
    <Header size={UISize.Small}>
        <div className="col pt-4 pb-4">
            <h1 className="display-3">Settings</h1>
        </div>
    </Header>
      <main className="container">
      {service.status === 'loading' && <div className="alert alert-purple" role="alert"><i className="fas fa-spinner"></i> Loading...</div>}
      {service.status === 'error' && (
        <div className="alert alert-danger" role="alert"><i className="fas fa-times"></i> Error, the backend moved to the dark side.</div>
      )}
      {service.status === 'loaded' && 
        
        <section className="pt-4 pb-5" data-aos="fade-up">
                <div className="card mb-4">
                <div className="card-body">
                  <h4 className="card-title">API Key</h4>

                <p>Pass this key in the <code>x-bolt-api-key</code> header when submitting new comments from your site.</p>
                <form onSubmit={handleSubmit(onSubmit)} className="needs-validation" noValidate>
                    <div className="form-row">
                        <div className="form-group col-7">
                            <input className={'form-control '+(!!errors?.apiKey?'is-invalid':'is-valid')} placeholder="Enter API key" {...register("apiKey", { required: true, minLength: { value:10, message: "Please add more characters..." } })}   />
                            <div className="invalid-feedback">{errors?.apiKey?.message}</div>
                            <small id="apiKeyHelp" className="form-text text-muted"> Make sure it's long at least 10 characters long.</small>
                        </div>
                        <div className="col-2">
                            <button type="submit" className="btn btn-success btn-round">Save</button>
                        </div>
                    </div>
                </form>
                </div>
                </div>
                <div className="card">
                <div className="card-body">
                  <h4 className="card-title">Web Hooks</h4>
                <form onSubmit={handleSubmit(onSubmit)} className="needs-validation" noValidate>
                    <div className="form-row">
                        <div className="form-group col-7">
                            <label htmlFor="webHookNewComment" className="text-uppercase">Comment added</label>
                            <input className={'form-control '+(!!errors?.webHookNewComment?'is-invalid':'is-valid')} placeholder="https://myhook.pipedream.net" {...register("webHookNewComment", { validate: { url: value => validateUrl(value) } } )}  />
                            <div className="invalid-feedback">{errors?.webHookNewComment?.message}{errors?.webHookNewComment?.type === 'url' && ' Please enter a valid URL starting with https://'}</div>
                            <p id="webHookNewCommentsHelp" className="form-text text-muted">This webhook will be invoked when a new comment is submitted to your site.</p>
                        </div>
                    </div>
                    <div className="form-row">
                        <div className="form-group col-7">
                            <label htmlFor="webHookNewComment" className="text-uppercase">Content changed</label>                           
                            <input className={'form-control '+(!!errors?.webHookCommentPublished?'is-invalid':'is-valid')} placeholder="https://myhook.pipedream.net" {...register("webHookCommentPublished", { validate: { url: value => validateUrl(value) } } )}  />
                            <div className="invalid-feedback">{errors?.webHookCommentPublished?.message}{errors?.webHookCommentPublished?.type === 'url' && ' Please enter a valid URL starting with https://'}</div>
                            <p className="form-text text-muted">This is called when a new comment is approved, rejected or deleted, but only if that means it's visibility on your site has changed. Use this to trigger content generation if you're rendering the comments on your site with a static site generator.</p>
                        </div>
                    </div>
                    <div className="form-row">
                      <div className="col-2">
                            <button type="submit" className="btn btn-success btn-round">Save</button>
                      </div>
                    </div>
                </form>
                </div>
                </div>
            
        </section>
    }
      </main>
    </>
  );
}


