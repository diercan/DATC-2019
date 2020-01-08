import json
import boto3

def lambda_handler(event, context):
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('Candidati')

    response = dynamoTable.get_item(
            Key = event
    )
    
    candidate_not_found = {
        'message': "Candidatul cu datele " + str(event) + " nu a fost gasit"
    }
    
    if "Item" not in response:
        return {
            'statusCode': 401,
            'body': json.dumps(candidate_not_found)
        }    
    
    return {
        'statusCode': 200,
        'body': json.dumps(response["Item"])
    }

