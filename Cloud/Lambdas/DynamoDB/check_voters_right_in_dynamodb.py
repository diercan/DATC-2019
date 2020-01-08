import json
import boto3

def lambda_handler(event, context):
    
    event = event["body-json"]
    COL_CNP = "CNP"
    
    dynamoDB = boto3.resource('dynamodb')
    
    dynamoVotanti = dynamoDB.Table('Votanti')
    dynamoCNPDB = dynamoDB.Table('CNPDataBase')

    max_cifre_cnp = 13

    cnp = event["CNP"]
        
    cnp_gresit = {
        'message':'false'
    }
        
    no_right_to_vote = {
        'message': 'false'
    }
    
    already_voted = {
        'message': 'false'
    }
    
    get_voter_in_valid_voters = dynamoCNPDB.get_item(
            Key = {
                "CNP": cnp
            }
        )    
    
    get_voter_in_already_voted_table = dynamoVotanti.get_item (
            Key = {
                "CNP": cnp
            }
        )

    if "Item" not in get_voter_in_valid_voters:
        return {
            
            'statusCode': 400,
            'body': json.dumps(no_right_to_vote)
            
        }
    
    if "Item" in get_voter_in_already_voted_table:
        return {
            'statusCode': 400,
            'body': json.dumps(already_voted)
        }
    
    
    if not event[COL_CNP].isdigit() or len(event[COL_CNP]) is not max_cifre_cnp:
        return {
            'statusCode': 403,
            'body': json.dumps(cnp_gresit)
        }
    
    return {
        'statusCode': 200,
        'body': json.dumps('true')
    }

