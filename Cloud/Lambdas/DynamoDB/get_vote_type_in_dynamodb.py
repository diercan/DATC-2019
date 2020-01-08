import json
import boto3

def lambda_handler(event, context):
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('VotingType')
    
    response = dynamoTable.scan()
    
    return {
        'statusCode': 200,
        'body': json.dumps(response["Items"])
    }

