import React from 'react';
import {
  Container, Col, Form,
  FormGroup, Label, Input,
  Button,
} from 'reactstrap';

export class AddAnimal extends React.Component {
    constructor(props) {
        super(props);
        this.state = {value: ''};

        this.handleChange = this.handleChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);   
    }

    handleChange(event){
        this.setState({value: event.target.value});
    }

    handleSubmit(event){
        alert('A name was submitted: ' + this.state.value);
        event.preventDefault()
    }

    render() {
        return (
          <Container className="AddAnimal">
          <h2>Add an animal</h2>
          <Form className="form">
            <Col>
              <FormGroup>
                <Label>Animal id</Label>
                <Input
                  type="number"
                  name="animalId"
                  id="animalId"
                  placeholder="1"
                />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label for="AnimalName">Animal name</Label>
                <Input
                  type="text"
                  name="animalName"
                  id="animalName"
                  placeholder="George"
                />
              </FormGroup>
            </Col>
            <Col>
              <FormGroup>
                <Label for="AnimalType">Animal type</Label>
                <Input type="select" name="animaType" id="animalType">
                  <option>Bear</option>
                  <option>Fox</option>
                  <option>Wolf</option>
                </Input>
              </FormGroup>
            </Col>
            <Button onClick={this.handleSubmit}>Submit</Button>
          </Form>
        </Container>
        );
      }
}