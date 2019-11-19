import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
  }

    componentDidMount() {       
        this.populateAnimalsData();
  }

  static renderForecastsTable(animals) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Id</th>
            <th>Name</th>
            <th>Location</th>
          </tr>
        </thead>
        <tbody>
                {animals.map(animals =>
                    <tr key={animals.id}>
                        <td>{animals.id}</td>
                        <td>{animals.name}</td>   
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
        ? <p><em>Loading...</em></p>
        : FetchData.renderForecastsTable(this.state.animals);

    return (
      <div>
        <h1 id="tabelLabel" >Animals</h1>
        <p>This component demonstrates fetching data from the server.</p>
        {contents}
      </div>
    );
  }

    async populateAnimalsData() {
    const response = await fetch('http://localhost:56148/api/Animal');        
    const data = await response.json();
    this.setState({ animals: data, loading: false });
  }
}
