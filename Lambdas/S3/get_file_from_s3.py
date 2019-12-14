import json
import boto3
import base64

def lambda_handler(event, context):
    
    bucket_name = 'proj-datc-bucket'
    
    s3 = boto3.client('s3')
    
    file_to_retrieve = event["Imagine"]

    stream = s3.get_object(Bucket=bucket_name, Key=file_to_retrieve)
    image_data = stream["Body"].read()
    
    encoded_string = str(base64.b64encode(image_data))
    
    return {
        'statusCode': 200,
        'body': json.dumps(encoded_string)
    }

