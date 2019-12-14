import json
import boto3

def lambda_handler(event, context):
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('Candidati')
    
    new_varsta = event["Varsta"]
    
    dynamoTable.update_item(
        Key={
            "Partid": event["Partid"],
            "NumeCandidat": event["NumeCandidat"]
        },
        UpdateExpression="set Varsta = :v, Proiect = :p",
        ExpressionAttributeValues={
            ':v': event["Varsta"],
            ':p': event["Proiect"]
        },
        ReturnValues="UPDATED_NEW"
    )
    
    message = {
        'message':"Datele actualizate cu success!"
    }
    
    return {
        'statusCode': 200,
        'body': json.dumps(message)
    }

