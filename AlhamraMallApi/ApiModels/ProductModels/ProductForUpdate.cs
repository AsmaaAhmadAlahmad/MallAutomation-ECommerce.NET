namespace AlhamraMallApi.ApiModels.ProductModels
{
    public class ProductForUpdate
    {
        public string? Name { get; set; }
        public required Guid CategoryId { get; set; }
        public required decimal Price { get; set; }
        public string? Description { get; set; }
        public required Guid CommercialStoreId { get; set; }

    }
}