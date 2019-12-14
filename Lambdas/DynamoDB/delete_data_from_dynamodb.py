import json
import boto3

def lambda_handler(event, context):
    
    COL1 = "Partid"
    COL2 = "NumeCandidat"
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('Candidati')
    
    keys_passed_not_needed = {
        'message': "[ ERROR ] Elementele din DB se sterg in functie " 
        + "de 'Partid' si 'NumeCandidat', nu in functie de " + str(event)
    }

    if COL1 not in event or COL2 not in event:
        return {
            'statusCode': 401,
            'body': json.dumps(keys_passed_not_needed)
        }

    dynamoTable.delete_item (
        Key = event
    )
    
    message = {
        'message': 'Informatii sterse cu success!'
    }
    
    return {
        'statusCode': 200,
        'body': json.dumps(message)
    }

