import json
import boto3

def lambda_handler(event, context):
    
    COL1 = "Partid"
    COL2 = "NumeCandidat"
    COL3 = "Varsta"
    
    max_cifre_varsta = 2
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('Candidati')
    
    partid_gresit = {
        'message': 'Numele partidului trebuie sa contina doar litere'
    }
    
    nume_gresit = {
        'message': 'Numele candidatului trebuie sa contina doar litere'
    }
    
    varsta_gresita = {
        'message': 'Varsta candidatului trebuie sa contina doar cifre (doar 2 cifre)'
    }
    
    if not event[COL1].isalpha():
        return {
            'statusCode': 401,
            'body': json.dumps(partid_gresit)
        }
        
    if not event[COL2].isalpha():
        return {
            'statusCode':402,
            'body': json.dumps(nume_gresit)
        }
        
    if not event[COL3].isdigit() or len(event[COL3]) is not max_cifre_varsta:
        return {
            'statusCode': 403,
            'body': json.dumps(varsta_gresita)
        }
    
    
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

