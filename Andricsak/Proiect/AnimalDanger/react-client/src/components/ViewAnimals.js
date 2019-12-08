import React, { Component } from 'react';
import { Map, TileLayer, Marker, Popup } from 'react-leaflet';


export class ViewAnimals extends  Component {
  
  constructor(props) {
    super(props);
    this.state = { animalList: [], loading: true ,zoom: 13};
  }

  componentDidMount() {
    this.populateAnimalData();
  }

  async populateAnimalData() {
    const response = await fetch('https://localhost:44339/api/Animal');
    const data = await response.json();
    console.log(data);
    this.setState({ animalList: data, loading: false });
    
  }
  render() {
    const centerPos = [45.748871,21.208679];
    console.log(this.state.animalList);
    return (

      <Map center={centerPos} zoom={this.state.zoom}
      style={{ height: "88vh" }} >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        {this.state.animalList.map(animal =>       
        <Marker position={[animal.lat,animal.long]} key={animal.id}>     
          <Popup>
            Id: {animal.id} <br /> Name: {animal.name}, Type: {animal.partitionKey}
          </Popup>
        </Marker>
        )}
      </Map>
    )
  }
}

