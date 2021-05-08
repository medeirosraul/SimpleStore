using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.CatalogItems;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Framework.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Products
{
    public interface ICatalogItemPictureService : IStoreBaseService<CatalogItemPicture>
    {

    }
    public class CatalogItemPictureService : StoreBaseService<CatalogItemPicture>, ICatalogItemPictureService
    {
        private readonly IPictureService _pictureService;
        private readonly IStorageObjectService _storageObjectService;

        public CatalogItemPictureService(StoreDbContext context, IStoreContext storeContext, IPictureService pictureService, IStorageObjectService storageObjectService) : base(context, storeContext)
        {
            _pictureService = pictureService;
            _storageObjectService = storageObjectService;
        }

        public override async Task<int> Insert(CatalogItemPicture picture)
        {
            var count = await base.Insert(picture);

            if (count == 0)
                return 0;

            // Rename filename with ID
            var extension = picture.Picture.FileName.Split('.').Last();
            picture.Picture.FileName = picture.Picture.FileName.Replace($".{extension}", string.Empty);
            picture.Picture.FileName += $"__{picture.Picture.Id}.jpg";


            return await Update(picture);
        }
    }
}