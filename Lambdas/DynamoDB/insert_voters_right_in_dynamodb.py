import json
import boto3

def lambda_handler(event, context):
    
    event = event["body-json"]
    
    COL2 = "CNP"
    
    max_cifre_cnp = 13
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('CNPDataBase')
        
    cnp_gresit = {
        'message':'CNP-ul trebuie sa contina 13 cifre'
    }
        
    if not event[COL2].isdigit() or len(event[COL2]) is not max_cifre_cnp:
        return {
            'statusCode': 403,
            'body': json.dumps(cnp_gresit)
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

