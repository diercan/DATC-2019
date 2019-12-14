import json
import boto3

def lambda_handler(event, context):
    
    dynamoDB = boto3.resource('dynamodb')
    
    dynamoVotanti = dynamoDB.Table('Votanti')
    dynamoCandidati = dynamoDB.Table('Candidati')

    judet = event["Judet"]
    cnp = event["CNP"]
    
    partid = event["Partid"]
    nume_candidat = event["NumeCandidat"]

    max_litere_judet = 2
    max_cifre_cnp = 13
    
    judet_gresit = {
        'message':'Judetul trebuie sa fie format doar din doua litere'
    }
    
    cnp_gresit = {
        'message':'CNP-ul trebuie sa fie format doar din 13 cifre'
    }
    
    if len(judet) is not max_litere_judet or not judet.isalpha():
        return {
            'statusCode': 401,
            'body': json.dumps(judet_gresit)
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
    
    varsta = response["Item"]["Varsta"]
    proiect = response["Item"]["Proiect"]
    voturi = response["Item"]["Voturi"]
    
    voturi = int(voturi) + 1
    
    dynamoCandidati.update_item(
        Key={
            "Partid": partid,
            "NumeCandidat": nume_candidat
        },
        UpdateExpression="set Varsta = :v, Proiect = :p, Voturi = :t",
        ExpressionAttributeValues={
            ':v': varsta,
            ':p': proiect,
            ':t': str(voturi)
        },
        ReturnValues="UPDATED_NEW"
    )
    
    dynamoVotanti.put_item(
        Item = {
            'Judet': judet,
            'CNP': cnp
        }
    )
    
    message = {
        'message':'Votare efectuata cu success!'
    }
    
    return {
        'statusCode': 200,
        'body': json.dumps(message)
    }

