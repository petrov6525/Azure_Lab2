using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Runtime.CompilerServices;

namespace Azure_Lab2.Models
{
    public class AzureService
    {
        private static  string connection = "DefaultEndpointsProtocol=https;AccountName=daryncevstudent;AccountKey=Di2rFiceS4dsU8ghZkk18EcY/CacChk4QLK8keOgLyLQH8S+XuG/6ngZvEFLarD0fYfM33Pvq+0i+ASto3vfTg==;EndpointSuffix=core.windows.net";
        
        private static BlobServiceClient blobServiceClient;


        public static async Task DownloadFile(string fileName)
        {
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("home");
            BlobClient blobClient = containerClient.GetBlobClient(fileName);


            using (MemoryStream stream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(stream);
                byte[] bytes=stream.ToArray();

                await File.WriteAllBytesAsync($"wwwroot/img/{fileName}", bytes);
            }
        }

        public static async Task UploadFile(IFormFile file)
        {
            blobServiceClient = new BlobServiceClient(connection);

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("home");

            BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

            using (Stream stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
            }
        }
        
        
        public  static async Task<bool> TryCreateBlobContainer(string containerName)
        {
            try
            {
                blobServiceClient=new BlobServiceClient(connection);
                
                BlobContainerClient containerClient= await blobServiceClient.CreateBlobContainerAsync(containerName);
                return true;
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }

        }
    }
}
