import json
import boto3
import base64

def lambda_handler(event, context):
    
    s3 = boto3.resource('s3')
    bucket = s3.Bucket('proj-datc-bucket')

    path_test = '/tmp/output'

    body_json = 0

    event_keys = []
    for key in event:
        event_keys.append(key)

    data_in_file = event[event_keys[0]]
    
    body_data = data_in_file.split(",")
    key = body_data[0].split(":")[1][1:-1]
    data = body_data[1].split(":")[1][1:-1]
    
    # key = event['ImageName']
    # data = event'img64']
    
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

