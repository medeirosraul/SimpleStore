using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Pictures;
using SimpleStore.Framework.Contexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Pictures
{
    public interface IPictureService: IStoreBaseService<Picture>
    {
        Task<Picture> GetByIdWithStorageObject(string id);
    }

    public class PictureService : StoreBaseService<Picture>, IPictureService
    {
        private readonly IStorageObjectService _storageObjectService;

        public PictureService(StoreDbContext context, IStoreContext storeContext, IStorageObjectService storageObjectService) : base(context, storeContext)
        {
            _storageObjectService = storageObjectService;
        }

        public async Task<Picture> GetByIdWithStorageObject(string id)
        {
            var pic = await GetById(id);
            if (pic == null)
                throw new NullReferenceException("Picture doesn't exists.");

            pic.StorageObject = await _storageObjectService.GetById(pic.StorageObjectId);

            return pic;
        }

        public override async Task<int> Insert(Picture picture, bool saveChanges = true)
        {
            var count = await base.Insert(picture);

            if (count == 0)
                return 0;

            // Rename filename with ID
            var extension = picture.FileName.Split('.').Last();
            picture.FileName = picture.FileName.Replace($".{extension}", string.Empty);
            picture.FileName += $"__{picture.Id}.{extension}";

            return await Update(picture);
        }
    }
}