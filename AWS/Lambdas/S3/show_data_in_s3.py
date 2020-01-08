import json
import boto3
import base64

def lambda_handler(event, context):
    
    s3 = boto3.resource('s3')
    bucket = s3.Bucket('proj-datc-bucket')
    
    keys = []
    
    for key in bucket.objects.all():
        keys.append(key)
    
    message = {
        'Data': str(keys)
    }
    
    return {
        'statusCode': 200,
        'body': json.dumps(message)
    }

