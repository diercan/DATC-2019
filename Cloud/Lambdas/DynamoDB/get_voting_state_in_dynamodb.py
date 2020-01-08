import json
import boto3

def lambda_handler(event, context):
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('VoteState')
    
    response = dynamoTable.scan()
    data = str(response["Items"]).split(":")[1].replace("'", "").replace("]", "").replace("}", "").strip()
    
    print(data)
    
    return {
        'statusCode': 200,
        'body': json.dumps(response["Items"])
    }

