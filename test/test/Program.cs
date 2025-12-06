using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace test
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            Console.WriteLine($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\kot2.jpg")}");

            /*
            Console.WriteLine("dodaje zdjecie: ");

            var url = "C://Users//KacperSz//Desktop//projekt bazy danych//test//kot2.jpg";
            //var imageUrl = await AddProductAsync(url);
            //Console.WriteLine($"to link: {imageUrl}");


            string text = "https://res.cloudinary.com/dv1nk0kbi/image/upload/v1764983236/emlighiq5swsg2ntlk4a.webp";

            //var nowy = text.Split('/').Last().Split('.').First().ToString();
            //var test = Path.GetFileNameWithoutExtension(text);

            //Console.WriteLine(nowy + "," + test);
            var image = await AddProductAsync(text);

            Console.WriteLine("juz");
            */
        }

        public static async Task<string> AddProductAsync(string imagePath)
        {
            //if (!File.Exists(imagePath))
            //{
            //    throw new FileNotFoundException($"File not found at path: {imagePath}");
            //}

            var _account = new Account(
                "dv1nk0kbi",
                "989932878854628",
                "U0TsweDyygH_mYnfGelE-I_MIzI");

            var _cloudinary = new Cloudinary(_account);
            _cloudinary.Api.Secure = true;

            //var uploadParams = new ImageUploadParams()
            //{
            //    File = new FileDescription(imagePath),
            //    //Folder = "projekt_bazy_danych"
            //};

            //var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            //if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            //    throw new Exception("Error occurred while uploading photo");

            //var ImageUrl = uploadResult.SecureUrl.ToString();

            //return ImageUrl;

            var publicId = Path.GetFileNameWithoutExtension(imagePath);

            var deletionParams = new DeletionParams(publicId);
            var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.Result != "ok")
            {
                throw new Exception("Deletion failed");
            }

            return "ok";
        }
    }
}
