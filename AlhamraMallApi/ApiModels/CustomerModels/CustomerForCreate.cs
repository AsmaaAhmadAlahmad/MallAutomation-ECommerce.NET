using System.ComponentModel.DataAnnotations;

namespace AlhamraMallApi.ApiModels.CustomerModels
{
    public class CustomerForCreate
    {
        [Required]
        public required string Name { get; set; }


        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string City { get; set; }

        [Required]
        public required string Country { get; set; }

        [Required]
        public required string PostalCode { get; set; }

        [Required]
        public required string ShippingAdress { get; set; }
    }
}