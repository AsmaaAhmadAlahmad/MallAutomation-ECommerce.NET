namespace AlhamraMallApi.ApiModels.CustomerModels
{
    public class CustomerForUpdate
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string City { get; set; }
        public required string Country { get; set; }
        public required string PostalCode { get; set; }
        public required string ShippingAdress { get; set; } // عنوان التوصيل
    }
}