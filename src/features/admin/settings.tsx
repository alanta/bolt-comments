import React, { useEffect } from "react";
import Header, { UISize } from "components/header";
import { useForm } from "react-hook-form";
import useUpdateSettings, { useSettingsService } from "services/settings.service";

type Inputs = {
  apiKey: string
};

export default function Settings(props: any) {
    const { register, handleSubmit, setValue, formState: { errors } } = useForm<Inputs>();
    const service = useSettingsService();
    const updateService = useUpdateSettings();
    const onSubmit = (data:Inputs) => updateService.update(data);
    
    useEffect(() => {
        document.title = "Settings - Bolt Comments"
    }, []);

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
        {
            setValue("apiKey", service.payload.apiKey)
        }    
                <h4 className="mb-4 h5">API Key</h4>
                <p>Pass this key in the <code>x-bolt-api-key</code> header when submitting new comments from your site.</p>
                <form onSubmit={handleSubmit(onSubmit)} className="needs-validation" noValidate>
                    <div className="form-row">
                        <div className="form-group col-7">
                            <input className="form-control" placeholder="Enter API key" {...register("apiKey", { required: true, minLength: { value:10, message: "It's not long enough..." } })}  />
                            <div className="invalid-feedback">{errors?.apiKey?.message}</div>
                            <small id="apiKeyHelp" className="form-text text-muted"> Make sure it's long at least 10 characters long.</small>
                        </div>
                        <div className="col-2">
                            <button type="submit" className="btn btn-success btn-round">Save</button>
                        </div>
                    </div>
                </form>
            
        </section>
    }
      </main>
    </>
  );
}


