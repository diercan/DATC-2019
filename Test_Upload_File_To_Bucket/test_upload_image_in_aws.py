import boto3 as b3
import csv


class BucketWrapper:

    def __init__(self, aws_id_key, aws_secret_key):
        self.aws_id_key = aws_id_key
        self.aws_secret_key = aws_secret_key

    def init_resources(self):
        s3 = b3.resource('s3', aws_access_key_id=self.aws_id_key,
                         aws_secret_access_key=self.aws_secret_key)
        return s3


class CfgWrapper:

    def __init__(self, cfg_file_name):
        self.cfg_file_name = cfg_file_name
        self.data_separator = ":"

    def get_data_on_line(self, line):
        return line.split(self.data_separator)[1].strip().strip("\"")

    def get_config_data(self):

        csv_file_name = access_key_id_header = secret_id_header = bucket_name = file_to_upload = ""

        cfg_file = open(self.cfg_file_name)

        for line in cfg_file:
            if "CSV_FILE_NAME" in line:
                csv_file_name = self.get_data_on_line(line)
            if "ACCESS_KEY_ID_HEADER" in line:
                access_key_id_header = self.get_data_on_line(line)
            if "ACCESS_SECRET_ID_HEADER" in line:
                secret_id_header = self.get_data_on_line(line)
            if "BUCKET_NAME" in line:
                bucket_name = self.get_data_on_line(line)
            if "FILE_TO_UPLOAD" in line:
                file_to_upload = self.get_data_on_line(line)

        return csv_file_name, access_key_id_header, secret_id_header, bucket_name, file_to_upload


def main():

    cfg_file_name = "upload_file.cfg"
    cfg = CfgWrapper(cfg_file_name)
    csv_file_name, access_key_id_header, secret_id_header, bucket_name, file_to_upload = cfg.get_config_data()

    access_key_id = ''
    secret_access_key = ''

    with open(csv_file_name, mode='r') as csv_file:
        csv_reader = csv.DictReader(csv_file)

        for row in csv_reader:
            for (key, value) in row.items():
                if access_key_id_header in key:
                    access_key_id = value
                if secret_id_header in key:
                    secret_access_key = value

    s3_controller = BucketWrapper(access_key_id, secret_access_key)
    s3 = s3_controller.init_resources()

    print("File will be uploaded in bucket: " + bucket_name)

    image = open(file_to_upload, "rb")
    s3.Bucket(bucket_name).put_object(Key=file_to_upload, Body=image)

    print("Done uploading file...")


if __name__ == "__main__":
    main()
