using SimpleStore.Core.Entities.CatalogItems;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SimpleStore.Web.Areas.Admin.Models.CatalogItems
{
    public class CatalogItemViewModel
    {
        public string Id { get; set; }
        public bool Published { get; set; }

        [Required]
        public CatalogItemType Type {get;set;}

        [Required]
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Sku { get; set; }
        public string Gtin { get; set; }

        public CatalogItemPriceViewModel ActivePrice { get; set; }

        public ICollection<CatalogItemPriceViewModel> Prices { get; set; }

        public ICollection<CatalogItemPictureViewModel> Pictures { get; set; }

        public CatalogItemViewModel()
        {
            SetActivePrice(string.Empty);
        }

        public CatalogItemViewModel(CatalogItem p)
        {
            FromCatalogItem(p);
            SetActivePrice(string.Empty);
        }

        public CatalogItemViewModel FromCatalogItem(CatalogItem p)
        {
            Id = p.Id;
            Published = p.Published;
            Type = p.Type;
            Name = p.Name;
            ShortDescription = p.ShortDescription;
            FullDescription = p.FullDescription;
            Sku = p.Sku;
            Gtin = p.Gtin;
            Prices = p.Prices?.Select(s => new CatalogItemPriceViewModel().FromPrice(s)).OrderByDescending(p => p.Active).ToList();
            Pictures = p.Pictures?.Select(s => new CatalogItemPictureViewModel().FromCatalogItemPicture(s)).ToList();
            SetActivePrice(string.Empty);
            return this;
        }

        public CatalogItem ToCatalogItem()
        {
            return new CatalogItem
            {
                Id = Id,
                Published = Published,
                Type = Type,
                Name = Name,
                ShortDescription = ShortDescription,
                FullDescription = FullDescription,
                Sku = Sku,
                Gtin = Gtin,
                Prices = Prices?.Select(s => s.ToPrice()).ToList(),
                Pictures = Pictures?.Select(s => s.ToCatalogItemPicture()).ToList()
            };
        }

        public void SetActivePrice(string id)
        {
            Prices ??= new List<CatalogItemPriceViewModel>();
            if (Prices.Count == 0)
                Prices.Add(new CatalogItemPriceViewModel());

            if (!string.IsNullOrEmpty(id))
            {
                ActivePrice = Prices.FirstOrDefault(p => p.Id == id);
                if (ActivePrice == null)
                    ActivePrice = Prices.OrderByDescending(p => p.Active)
                    .FirstOrDefault();
            } 
            else
                ActivePrice = Prices.OrderByDescending(p => p.Active)
                    .FirstOrDefault();
        }
    }
}
