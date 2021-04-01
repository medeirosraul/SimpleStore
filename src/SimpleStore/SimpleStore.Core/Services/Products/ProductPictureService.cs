using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.CatalogItems;
using SimpleStore.Core.Services.Pictures;
using SimpleStore.Framework.Contexts;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.Products
{
    public interface IProductPictureService: IStoreBaseService<CatalogItemPicture>
    {

    }
    public class ProductPictureService : StoreBaseService<CatalogItemPicture>, IProductPictureService
    {
        private readonly IPictureService _pictureService;
        private readonly IStorageObjectService _storageObjectService;

        public ProductPictureService(StoreDbContext context, IStoreContext storeContext, IPictureService pictureService, IStorageObjectService storageObjectService) : base(context, storeContext)
        {
            _pictureService = pictureService;
            _storageObjectService = storageObjectService;
        }

        public override async Task<int> InsertOrUpdate(CatalogItemPicture picture)
        {
            TrackEntity(picture);
            _pictureService.TrackEntity(picture.Picture);
            _storageObjectService.TrackEntity(picture.Picture.StorageObject);

            // Change Picture Name
            var extension = picture.Picture.FileName.Split('.').Last();
            picture.Picture.FileName = picture.Picture.FileName.Replace($".{extension}", string.Empty);
            picture.Picture.FileName += $"__{picture.Picture.Id}.jpg";

            return await SaveChanges();
        }
    }
}