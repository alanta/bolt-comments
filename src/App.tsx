import React, { useEffect } from 'react';
import { 
  BrowserRouter as Router,
  Switch,
  Route,  
  Redirect
} from "react-router-dom";
import Footer from 'components/footer'
import Navbar from 'components/navbar'
import Home from 'features/home/home'
import Approvals from 'features/comments/approvals'
import Comments from 'features/comments/comments'
import './App.scss';

import AOS from "aos";
import "aos/dist/aos.css";
import Login from 'features/account/login';
import { useApprovalsService } from 'services/comments.service';
import Settings from 'features/admin/settings';
import { ProvideAuth, useAuth } from 'services/auth.service';
import Unauthorized from 'features/account/unauthorized';
AOS.init();

function App() {
  useEffect(() => {
    AOS.init({
      duration: 700,
      disable: function () {
        //Disable animation on less than 1200px
        var maxWidth = 1200;
        return window.innerWidth < maxWidth;
      }
    });
    AOS.refresh();
  }, []);

  const { service, removeItem, refresh } = useApprovalsService();

  return (
    <Router>
    
    <div className="App">
      <ProvideAuth>
        <Navbar service={service} />
        <Switch>
          <Route exact path="/">
            <Home />
          </Route>
          <Route path="/approvals">
            <Approvals approvalsService={service} removeItem={removeItem} refresh={refresh} />
          </Route>
          <Route path="/comments">
            <Comments refresh={refresh} />
          </Route>
          <PrivateRoute path="/settings" roles={["admin"]}>
            <Settings />
          </PrivateRoute>
          <Route path="/login" >
            <Login />
          </Route>
          <Route path="/unauthorized" >
            <Unauthorized />
          </Route>
        </Switch>
        <Footer />
      </ProvideAuth>
    </div>
    
   </Router>
  );
}

export const PrivateRoute: React.FC<{children?: React.ReactNode, path: string, roles?:string[] }> = (props) => {
  const auth = useAuth();
  
  var routeProps = {...props, children: null}

   return (
    <Route
     {...routeProps}
      render={({ location }) =>
        (auth.status === 'loaded' && auth.payload.authenticated && auth.payload.isInAnyRole(props.roles)) ? 
        (
          props.children
        ) 
        : ( auth.status === 'loaded' &&
          <Redirect
            to={{
              pathname: "/unauthorized",
              state: { from: location }
            }}
          />
        )
      }
    />
  );
}

export default App;
