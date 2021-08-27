using Microsoft.EntityFrameworkCore;
using SimpleStore.Core.Data;
using SimpleStore.Core.Entities.Catalog;
using SimpleStore.Framework.Contexts;

namespace SimpleStore.Core.Services.Catalog
{
    public interface ICatalogProductService : IStoreBaseService<CatalogProduct>
    {
        Task<int> InsertPicture(CatalogProductPicture picture);
        Task<int> DeletePicture(string id);

        Task UpdateProductStock(CatalogProduct product, int quantity);
    }

    public class CatalogProductService : StoreBaseService<CatalogProduct>, ICatalogProductService
    {
        private readonly IStoreBaseService<CatalogProductPicture> _productPictureService;

        public CatalogProductService(
            StoreDbContext context,
            IStoreContext storeContext,
            IStoreBaseService<CatalogProductPicture> productPictureService) : base(context, storeContext)
        {
            _productPictureService = productPictureService;
        }

        public override async Task<CatalogProduct> GetById(string id, bool tracking = false)
        {
            var query = PrepareQuery(tracking)
                .Where(p => p.Id == id)
                .Include(p => p.Pictures.Where(x => !x.Deleted))
                .ThenInclude(p => p.Picture);

            var result = await Get(query);
            return result.FirstOrDefault();
        }

        public async Task<int> InsertPicture(CatalogProductPicture picture)
        {
            var count = await _productPictureService.Insert(picture);

            if (count == 0)
                return 0;

            // Rename filename with ID
            var extension = picture.Picture.FileName.Split('.').Last();
            picture.Picture.FileName = picture.Picture.FileName.Replace($".{extension}", string.Empty);
            picture.Picture.FileName += $"__{picture.Picture.Id}.jpg";


            return await _productPictureService.Update(picture);
        }

        public Task<int> DeletePicture(string id)
        {
            return _productPictureService.Delete(id);
        }

        public async Task UpdateProductStock(CatalogProduct product, int quantity)
        {
            product.StockQuantity += quantity;
            await Update(product);
        }
    }
}