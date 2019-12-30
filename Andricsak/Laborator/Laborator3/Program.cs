using System; 
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.IO;
namespace Laborator2
{
    class Program
    {
        static string[] scopes = {DriveService.Scope.Drive,DriveService.Scope.DriveFile};
        private static string _token;
        static void Main(string[] args)
        {
            init();
        }

        static void init()
        {
            

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets{
                    ClientId = credentials[0],
                    ClientSecret = credentials[1]
                },
                scopes,
                Environment.UserName,
                CancellationToken.None,
                new FileDataStore("Credential",false)
            ).Result;
            
            //Token created
            Console.WriteLine("Token is "+ credential.Token.AccessToken);
   
            GetMyFiles(credential);
            //UploadFile(credential);
        }

        static void GetMyFiles(UserCredential credential){

            // create new driver service 
            var service = new DriveService(new BaseClientService.Initializer(){
                HttpClientInitializer = credential,
                ApplicationName = "DATC"
            });
             FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;
            Console.WriteLine("Files: ");
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    Console.WriteLine("{0} ({1})", file.Name);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            Console.Read();
        }
        static void UploadFile(UserCredential credential)
        {
            var service = new DriveService(new BaseClientService.Initializer(){
                HttpClientInitializer = credential,
                ApplicationName = "DATC"
            });

            var fileMetaData = new Google.Apis.Drive.v3.Data.File()
            {
              Name = "MyFolder",
              MimeType = "application/vnd.google-apps.folder"  
            };

            FilesResource.CreateMediaUpload request;

            using (var stream = new FileStream(("./test.txt"),System.IO.FileMode.Open))
            {
                request = service.Files.Create(
                fileMetaData, stream, "test");
                request.Fields = "id";
                request.Upload();
            };
            var file = request.ResponseBody;
            //Console.WriteLine("File ID: " + file.Id);   
        }
    }


}
