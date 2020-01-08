import json
import boto3

def lambda_handler(event, context):
    
    event = event["body-json"]
    
    dynamoDB = boto3.resource('dynamodb')
    dynamoTable = dynamoDB.Table('VotingType')
    
    # clear table
    scan = dynamoTable.scan()
    with dynamoTable.batch_writer() as batch:
        for each in scan['Items']:
            batch.delete_item(
                Key={
                    'StopCandidati': each['StopCandidati'],
                    'TipAlegeri': each['TipAlegeri']
                }
            )
            
    dynamoTable.put_item(
        Item = event
    )
    
    return {
        'statusCode': 200,
        'body': json.dumps('ok')
    }

