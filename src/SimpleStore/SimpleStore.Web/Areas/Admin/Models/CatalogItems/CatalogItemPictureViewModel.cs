using SimpleStore.Core.Entities.CatalogItems;
using SimpleStore.Web.Areas.Admin.Models.Pictures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Admin.Models.CatalogItems
{
    public class CatalogItemPictureViewModel
    {
        public string Id { get; set; }

        public PictureViewModel Picture { get; set; }

        public CatalogItemPictureViewModel FromCatalogItemPicture(CatalogItemPicture p)
        {
            Id = p.Id;
            if (p.Picture != null)
                Picture = new PictureViewModel().FromPicture(p.Picture);

            return this;
        }

        public CatalogItemPicture ToCatalogItemPicture()
        {
            var p = new CatalogItemPicture
            {
                Id = Id,
                Picture = Picture?.ToPicture()
            };

            return p;
        }
    }
}
