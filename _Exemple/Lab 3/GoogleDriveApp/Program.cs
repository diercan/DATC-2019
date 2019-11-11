using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace GoogleDriveApiApp
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/drive-dotnet-quickstart.json
        static string[] Scopes = { DriveService.Scope.Drive };
        static string ApplicationName = "Drive API .NET Quickstart";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials2.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token2.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            string filePath = "MyXmlFile.xml";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(credential.Token.TokenType, credential.Token.AccessToken);
            httpClient.BaseAddress = new Uri("https://www.googleapis.com");

            // Filtru in fiddler: googleapis.com
            DeleteNewFiles(service);

            // https://developers.google.com/drive/api/v3/quickstart/dotnet
            ListAllFilesFromDrive(service);

            // https://developers.google.com/drive/api/v3/manage-uploads
            CreateFileInDrive(service);

            // https://developers.google.com/drive/api/v3/reference/files
            ListAllFilesFromDriveHttpClient(httpClient);
            CreateFileInDriveHttpClient(httpClient);

            Console.ReadLine();
        }

        private static void DeleteNewFiles(DriveService driveService)
        {
            var files = driveService.Files.List().Execute();
            driveService.Files.Delete(files.Files.Where(file => file.Name == "testfile.xml")?.FirstOrDefault()?.Id).Execute();
            driveService.Files.Delete(files.Files.Where(file => file.Name == "testfilehttp.xml")?.FirstOrDefault()?.Id).Execute();
        }

        private static void CreateFileInDriveHttpClient(HttpClient httpClient)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(new CreateGoogleFile()
            {
                name = "testfilehttp.xml"
            }), Encoding.UTF8, "application/json");

            var response = httpClient.PostAsync("https://www.googleapis.com/upload/drive/v3/files?uploadType=resumable&fields=id", stringContent).Result;
            Console.WriteLine(response.StatusCode);
            using (var stream = new FileStream("testfile.xml",
                                    FileMode.Open))
            {
                using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    var text = streamReader.ReadToEnd();
                    var putreponse = httpClient.PutAsync(response.Headers.Location, new StringContent(text)).Result;
                    Console.WriteLine(putreponse.StatusCode);
                }
            }
        }

        private static void CreateFileInDrive(DriveService service)
        {
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "testfile.xml"
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream("testfile.xml",
                                    FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, "text/xml");
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
        }

        private static void ListAllFilesFromDriveHttpClient(HttpClient httpClient)
        {            
            var result = httpClient.GetAsync("/drive/v3/files").Result;
            Console.WriteLine(result.StatusCode);
            Console.ReadLine();
        }

        private static void ListAllFilesFromDrive(DriveService service)
        {
            var token = string.Empty;
            do
            {
                // Define parameters of request.
                FilesResource.ListRequest listRequest = service.Files.List();
                if (token != string.Empty)
                {
                    listRequest.PageToken = token;
                }

                listRequest.PageSize = 10;
                listRequest.Fields = "nextPageToken, files(id, name)";

                // List files.
                var result = listRequest.Execute();
                IList<Google.Apis.Drive.v3.Data.File> files = result.Files;

                Console.WriteLine("Files:");
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        Console.WriteLine("{0} ({1})", file.Name, file.Id);
                    }
                }
                else
                {
                    Console.WriteLine("No files found.");
                }

                token = result.NextPageToken;
                Console.ReadLine();
            } while (token != null);
        }
    }
}
