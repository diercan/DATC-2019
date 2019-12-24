import React, { Component } from 'react';
import { Map, TileLayer, Marker, Popup,Polygon } from 'react-leaflet';

// default safe zone 
const polygon = [
  [45.751714,21.219859],
	 [45.742850,21.219516],
	 [45.743676,21.236074],
	 [45.750864,21.233672],
]

export class ViewAnimals extends  Component {
  
  constructor(props) {
    super(props);
    this.state = { animalList: [], loading: true ,zoom: 13,alertsList: [],alert:false};
  }

  componentDidMount() {
    this.populateAnimalData(); 
    // setInterval(this.populateAnimalData,10000); 
  //  setInterval(this._handleAlert,1000);
  }

   populateAnimalData = async()=> {
    const response = await fetch('https://animaldangerapi.azurewebsites.net/api/Animal');
    const data = await response.json();
    console.log(data);
    this.setState({ animalList: data, loading: false });
  }

  _showAlert=(description)=>{

    if(this.state.alert === false){
 
      alert(`DANGER :${description}!`);
    }
  };
  
  // get the animals present in the area
 _handleAlert=async()=>{
  const response = await fetch('https://animaldangerapi.azurewebsites.net/api/Animal/Alerts');
  const data = await response.json();
  console.log(data);
  if(data != null){
    this._showAlert(data[0].description);
    this.setState({alert:true});
  }
  else
  {
    this.setState({alert:false})
  }
 };


  render() {
    const centerPos = [45.748871,21.208679];
    console.log(this.state.animalList)
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
        <Polygon color="purple" positions={polygon} />
      </Map>
    )
  }
}

