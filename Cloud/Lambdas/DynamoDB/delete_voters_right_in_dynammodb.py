import json
import boto3

def lambda_handler(event, context):
    
    COL_CNP = "CNP"
    max_cifre_cnp = 13
    
    event = event["body-json"]
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('CNPDataBase')
    
    keys_passed_not_needed = {
        'message': "false"
    }

    if COL_CNP not in event:
        return {
            'statusCode': 401,
            'body': json.dumps(keys_passed_not_needed)
        }

    dynamoTable.delete_item (
        Key = event
    )
    
    message = {
        'message': 'true'
    }
    
    if not event[COL_CNP].isdigit() or len(event[COL_CNP]) is not max_cifre_cnp:
        return {
            'statusCode': 403,
            'body': json.dumps(cnp_gresit)
        }
    
    return {
        'statusCode': 200,
        'body': json.dumps(message)
    }

