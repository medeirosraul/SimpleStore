using System.ComponentModel.DataAnnotations;

namespace SimpleStore.Web.Areas.Admin.Models.Store
{
    public class StoreViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Subdomínio")]
        public string Subdomain { get; set; }
    }
}
