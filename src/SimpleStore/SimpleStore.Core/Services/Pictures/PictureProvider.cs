using Microsoft.AspNetCore.Hosting;
using SimpleStore.Core.Entities.Pictures;
using SimpleStore.Framework.Contexts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Pictures
{
    public interface IPictureProvider
    {
        Task CreatePicturePath(string id, int size, string path, string filename);
        Task<string> GetPicturePath(string relativePath, string fileName, int size);
        string GetProductPictureUrl(Picture picture, int size);
    }

    public class PictureProvider : IPictureProvider
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IStoreContext _storeContext;
        private readonly IPictureService _pictureService;
        private readonly IStorageObjectService _storageObjectService;

        public PictureProvider(IWebHostEnvironment environment, IStoreContext storeContext, IPictureService pictureService, IStorageObjectService storageObjectService)
        {
            _environment = environment;
            _storeContext = storeContext;
            _pictureService = pictureService;
            _storageObjectService = storageObjectService;
        }

        public async Task<string> GetPicturePath(string relativePath, string fileName, int size)
        {
            var extension = fileName.Split('.').Last();
            var nameAndId = fileName.Replace($".{extension}", string.Empty);
            var id = nameAndId.Split("__").Last();
            var name = nameAndId.Replace($"__{id}", string.Empty);

            // Define file name
            var physicalFileName = string.Empty;

            if (size > 0)
                physicalFileName = $"{name}__{id}__{size}.jpg";
            else
                physicalFileName = $"{name}__{id}.jpg";

            // Define file path
            var path = Path.Combine(_environment.ContentRootPath, "App_Data", "Stores", _storeContext.CurrentStore.Subdomain, "Pictures", relativePath);
            var pathWithFilename = Path.Combine(path, physicalFileName);

            // Verifify if file with this name and size exists
            bool exists = await Task.Run(() => File.Exists(pathWithFilename));
            if (exists) return pathWithFilename;

            // Create picture in path
            await CreatePicturePath(id, size, path, physicalFileName);

            // return path
            return pathWithFilename;
        }

        public async Task CreatePicturePath(string id, int size, string path, string filename)
        {
            var picture = await _pictureService.GetByIdWithStorageObject(id);
            using var image = Image.Load(picture.StorageObject.Bytes);

            if (size > 0)
            {
                var width = image.Width >= image.Height ? size : 0;
                var height = image.Height >= image.Width ? size : 0;
                image.Mutate(x => x.Resize(width, height));
            }
            
            Directory.CreateDirectory(path);
            await image.SaveAsync(Path.Combine(path, filename), new JpegEncoder());
        }

        public string GetProductPictureUrl(Picture picture, int size)
        {
            if (picture == null) 
                return string.Empty;

            var url = $"https://{_storeContext.GetHost()}/Picture/Product/{size}/{picture.FileName}";
            return url;
        }
    }
}
