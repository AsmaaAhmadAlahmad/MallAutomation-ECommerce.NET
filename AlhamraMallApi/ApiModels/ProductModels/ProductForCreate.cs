using System.ComponentModel.DataAnnotations;

namespace AlhamraMallApi.ApiModels.ProductModels
{
    public class ProductForCreate
    {
       
        //public required string Name { get; set; }
        //public required decimal Price { get; set; }
        //public required Guid CategoryId { get; set; }
        //public required IFormFile File { get; set; }
        //public required string Description { get; set; }
        //public required Guid CommercialStoreId { get; set; }




        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "File is required")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "CommercialStore is required")]
        public Guid CommercialStoreId { get; set; }
    }



}