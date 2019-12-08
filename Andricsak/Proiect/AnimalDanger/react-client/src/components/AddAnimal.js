import React from 'react';
import {
  Container, Col, Form,
  FormGroup, Label, Input,
  Button,
} from 'reactstrap';
import axios from 'axios';

export class AddAnimal extends React.Component {
    constructor(props) {
        super(props);

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);   
    }

    handleChange(event){
        this.setState({value: event.target.value});
    }

    handleSubmit(event){
        const form = event.target;
        const dataToPost = new FormData(form);
  
        // empty object
        var object = {};
        dataToPost.forEach(function (value,key) {
          if(key == "name")
          {
            object[key] = value;
          }
          else
          {
            object[key] = Number(value);
          }
          
          console.log(key +": " + value)
        });
      
        var json =JSON.stringify(object);
        console.log(json)
        axios({
        method: 'post', 
        url: 'https://animaldangerapi.azurewebsites.net/api/Animal', 
        headers:{'Content-Type': 'application/json; charset=utf-8'},
        data: json  
        })
        .then(function (response) {
                console.log("The response of the server: "+response.data);
        })
        .catch(function (error) {
            // handle error
            console.log(error);
        })
        .finally(function () {
                //
        });
      
    }

    render() {
        return (
          <Container className="AddAnimal">
          <h2>Add an animal</h2>
          <Form className="form" onSubmit={this.handleSubmit}>
            <Col>
              <FormGroup>
                <Label>Animal id</Label>
                <Input
                  type="number"
                  name="id"
                  id="animalId"
                  placeholder="1"
                />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label>Latitude</Label>
                <Input
                  type="number"
                  name="lat"
                  placeholder="latitude..."
                  step="any"
                />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label>Longitude</Label>
                <Input
                  type="number"
                  name="long"
                  placeholder="longitutde..."
                  step="any"
                />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label for="AnimalName">Animal name</Label>
                <Input
                  type="text"
                  name="name"
                  id="animalName"
                  placeholder="Animal name"
                />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label for="AnimalType">Animal type</Label>
                <Input type="select" name="type" id="animalType">
                  <option value="0">Bear</option>
                  <option value="1">Fox</option>
                  <option value="2">Wolf</option>
                </Input>
              </FormGroup>
            </Col>
            <Button>Submit</Button>
          </Form>
        </Container>
        );
      }
}