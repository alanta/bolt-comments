import React, { useEffect } from 'react';
import { 
  BrowserRouter as Router,
  Switch,
  Route  
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

  return (
    <Router>
    
    <div className="App">
    <Navbar />
    <Switch>
      <Route exact path="/">
        <Home />
      </Route>
      <Route path="/approvals">
        <Approvals />
      </Route>
      <Route path="/comments">
        <Comments />
      </Route>
      <Route path="/login">
        <Login />
      </Route>
    </Switch>
    <Footer />
    </div>
    
   </Router>
  );
}

export default App;
