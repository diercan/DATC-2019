import React, { Component } from 'react';
import { Map, TileLayer, Marker, Popup } from 'react-leaflet';


  
export class ViewAnimals extends  Component {
  state = {
    lat: 45.746059,
    lng: 21.250750,
    zoom: 13,
  }

  render() {
    const position = [this.state.lat, this.state.lng]
    return (
      <Map center={position} zoom={this.state.zoom}
      style={{ height: "88vh" }} >
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        />
        <Marker position={position}>
          <Popup>
            A pretty CSS3 popup. <br /> Easily customizable.
          </Popup>
        </Marker>
      </Map>
    )
  }
}

  