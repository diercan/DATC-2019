import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Welcome to animal danger website!</h1>
        <p>Here you can view if dangerous animals are inside your city!</p>
        </div>
    );
  }
}
