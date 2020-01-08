import json
import boto3

def lambda_handler(event, context):
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('Candidati')
    
    response = dynamoTable.get_item(
            Key = {
                "Partid": event["Partid"],
                "NumeCandidat": event["NumeCandidat"]
            }
        )
    
    new_varsta = event["Varsta"]
    
    dynamoTable.update_item(
        Key={
            "Partid": event["Partid"],
            "NumeCandidat": event["NumeCandidat"]
        },
        UpdateExpression="set Varsta = :v, Proiect = :p, Voturi = :t",
        ExpressionAttributeValues={
            ':v': event["Varsta"],
            ':p': event["Proiect"],
            ':t': response["Item"]["Voturi"]
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

