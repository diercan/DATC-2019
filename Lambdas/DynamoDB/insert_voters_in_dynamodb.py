import json
import boto3

def lambda_handler(event, context):
    
    event = event["body-json"]
    
    # check if voter has already voted, if yes, deny vote
    # check if voter is in the table with valid voters
    dynamoDB = boto3.resource('dynamodb')
    
    dynamoVotanti = dynamoDB.Table('Votanti')
    dynamoCandidati = dynamoDB.Table('Candidati')
    
    # table with voters that have the right to vote
    dynamoCNPDB = dynamoDB.Table('CNPDataBase')

    cnp = event["CNP"]
    
    partid = event["Partid"]
    nume_candidat = event["NumeCandidat"]

    max_cifre_cnp = 13

    cnp_gresit = {
        'message':'false'
    }
    
    if len(cnp) is not max_cifre_cnp or not cnp.isdigit():
        return {
            'statusCode': 402,
            'body': json.dumps(cnp_gresit)
        }
        
    # get the amount of votes the candidate that votes has
    response = dynamoCandidati.get_item(
        Key = {
            "Partid": partid,
            "NumeCandidat": nume_candidat
        }
    )
    
    candidat_inexistent = {
        'message': "false"
    }
    
    if "Item" not in response:
        return {
            'statusCode': 406,
            'body': json.dumps(candidat_inexistent)
        }
    voturi = response["Item"]["Voturi"]
    
    voturi = int(voturi) + 1
    
    dynamoCandidati.update_item(
        Key={
            "Partid": partid,
            "NumeCandidat": nume_candidat
        },
        UpdateExpression="set Voturi = :t",
        ExpressionAttributeValues={
            ':t': str(voturi)
        },
        ReturnValues="UPDATED_NEW"
    )
    
    dynamoVotanti.put_item(
        Item = {
            'CNP': cnp
        }
    )
    
    message = {
        'message':'true'
    }
    
    return {
        'statusCode': 200,
        'body': json.dumps(message)
    }

