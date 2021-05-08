using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimpleStore.Web.Areas.Store.ViewModels.Catalog
{
    public class CatalogItemDetailsViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public CatalogItemPriceViewModel Price { get; set; }
        public ICollection<CatalogItemPictureViewModel> Pictures { get; set; }

        public CatalogItemDetailsViewModel()
        {
            Pictures = new Collection<CatalogItemPictureViewModel>();
        }
    }
}