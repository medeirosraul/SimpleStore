using SimpleStore.Core.Entities.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Admin.Models.CatalogItems
{
    public class CatalogItemPriceViewModel
    {
        public string Id { get; set; }
        public string CatalogItemId { get; set; }
        public decimal Value { get; set; }
        public decimal OldValue { get; set; }
        public decimal Cost { get; set; }
        public bool Active { get; set; }

        public CatalogItemPriceViewModel FromPrice(Price p)
        {
            Id = p.Id;
            CatalogItemId = p.CatalogItemId;
            Value = p.Value;
            OldValue = p.OldValue;
            Cost = p.Cost;
            Active = p.Active;

            return this;
        }

        public Price ToPrice()
        {
            return new Price
            {
                Id = Id,
                CatalogItemId = CatalogItemId,
                Value = Value,
                OldValue = OldValue,
                Cost = Cost,
                Active = Active
            };
        }
    }
}
