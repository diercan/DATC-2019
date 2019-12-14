import json
import boto3
import base64

def lambda_handler(event, context):
    
    s3 = boto3.resource('s3')
    bucket = s3.Bucket('proj-datc-bucket')
    
    path_test = '/tmp/output'

    key = event["body-json"]["ImageName"]
    data = event["body-json"]["img64"]
    
    data1 = data
    img = base64.b64decode(data1)
    
    with open(path_test, 'wb') as data:
        data.write(img)
        bucket.upload_file(path_test, key)
        
    message = {
        'message': 'Execution started successfully!'
    }
    
    return {
        'statusCode': 200,
        'headers': {'Content-Type': 'application/json'},
        'body': json.dumps(message)
    }

