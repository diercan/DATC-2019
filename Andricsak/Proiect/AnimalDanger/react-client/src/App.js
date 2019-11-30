import React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { AddAnimal } from './components/AddAnimal';
import { ViewAnimals } from './components/ViewAnimals';

export default class App extends React.Component {
  static displayName = App.name;

  render() {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/AddAnimal' component={AddAnimal} />
        <Route path='/ViewAnimals' component={ViewAnimals} />
      </Layout>
    )
  }
}
