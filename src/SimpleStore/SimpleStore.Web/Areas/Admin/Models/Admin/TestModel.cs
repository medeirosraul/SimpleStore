using SimpleStore.Core.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleStore.Web.Areas.Admin.Models.Admin
{
    public class TestModel
    {
        public string Text { get; set; }
        public int Integer { get; set; }
        public decimal Decimal { get; set; }
        public bool Boolean { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
