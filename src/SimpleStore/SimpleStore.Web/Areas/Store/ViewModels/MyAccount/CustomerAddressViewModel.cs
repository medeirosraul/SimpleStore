using System.ComponentModel.DataAnnotations;

namespace SimpleStore.Web.Areas.Store.ViewModels.MyAccount
{
    public class CustomerAddressViewModel
    {
        public string Id { get; set; }
        public bool IsShippingAddress { get; set; }

        [Required]
        public string Responsible { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Number { get; set; }

        public string Complement { get; set; }

        [Required]
        public string Neighborhood { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        public string Country { get; set; }
    }
}
