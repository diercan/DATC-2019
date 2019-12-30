import {Login} from './components/Login';
import React, {Component} from 'react';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { AddAnimal } from './components/AddAnimal';
import { ViewAnimals } from './components/ViewAnimals';
import { Route,Redirect } from 'react-router';
import { GoogleLogin,GoogleLogout } from 'react-google-login';
import PrivateRoute from './components/PrivateRoute';
import nature from './components/nature.jpeg';



export default class App extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isLoggedIn: false,
    }
  }

  render (){
    const responseGoogle = (response) => {
      console.log("Setting loggedin to true");
      this.setState({isLoggedIn: true})
    }
    const logout = (e) =>
    {
      this.setState({isLoggedIn: false})    
    }
    
    console.log(this.state.isLoggedIn);
    
    if(this.state.isLoggedIn === true)
    {
      return(
        <div className="App"> 
         <Layout>
              <PrivateRoute isLogged={this.state.isLoggedIn} component={Home} path="/Home" exact/>
              <PrivateRoute isLogged={this.state.isLoggedIn} component={AddAnimal} path="/AddAnimal" exact/>
              <PrivateRoute isLogged={this.state.isLoggedIn} component={ViewAnimals} path="/ViewAnimals" exact  />
              <GoogleLogout
          clientId="707554693086-onkdae6efl5saje0jj6n1h089n0j4ger.apps.googleusercontent.com"
          buttonText="LOGOUT"
          onLogoutSuccess={logout}
          >
          </GoogleLogout>
          </Layout>
        </div>
      )
    }else{
      return(
  <div>
   <h1>Welcome to animal danger administrator application!</h1>
    <GoogleLogin
          id="Login"
          clientId="707554693086-onkdae6efl5saje0jj6n1h089n0j4ger.apps.googleusercontent.com" //CLIENTID NOT CREATED YET
          buttonText="LOGIN WITH GOOGLE"
          onSuccess={responseGoogle}
          />
  </div>
      );
    }
  }
}


