using SimpleStore.Core.Entities.Pictures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Admin.Models.Pictures
{
    public class PictureViewModel
    {
        public string Id { get; set; }
        public string Path { get; set; }

        public string FileName { get; set; }

        public string Title { get; set; }

        public PictureViewModel FromPicture(Picture p)
        {
            Id = p.Id;
            Path = p.Path;
            FileName = p.FileName;
            Title = p.Title;

            return this;
        }

        public Picture ToPicture()
        {
            var picture = new Picture
            {
                Id = Id,
                Path = Path,
                FileName = FileName,
                Title = Title
            };

            return picture;
        }
    }
}
