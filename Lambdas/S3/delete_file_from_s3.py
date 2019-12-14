import json
import boto3

def lambda_handler(event, context):
    
    file_to_delete = event["Fisier"]
    
    bucket = 'proj-datc-bucket'
    
    s3 = boto3.client('s3')
    s3.delete_object(Bucket = bucket, Key = file_to_delete)
    
    message = {
        'message':"Fisierul a fost sters cu sucess!"
    }
    
    return {
        'statusCode': 200,
        'body': json.dumps(message)
    }

