using System.ComponentModel.DataAnnotations;

namespace AlhamraMallApi.ApiModels
{
    public class CustomerModel
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
        public required string ShippingAddress { get; set; }
    }
}
