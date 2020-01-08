import json
import boto3

def lambda_handler(event, context):
    
    event = event["body-json"]
    
    COL_nume = "NumeCandidat"
    COL_voturi = "Voturi"
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('Candidati')
    
    nume_multiple = event[COL_nume].split(" ")

    nume_gresit = {
        'message': 'Numele candidatului trebuie sa contina doar litere'
    }
    
    for nume in nume_multiple:
        if not nume.isalpha():
            return {
                'statusCode':402,
                'body': json.dumps(nume_gresit)
            }
    
    event[COL_voturi] = "0"
    
    dynamoTable.put_item(
        Item = event
    )
    
    message = {
        'message': 'Informatii salvate cu success!'
    }
    
    return {
        'statusCode': 200,
        'body': json.dumps(message)
    }

