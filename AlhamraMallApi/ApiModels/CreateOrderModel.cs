namespace AlhamraMallApi.ApiModels
{
    public class CreateOrderModel
    {
        public Guid? CartId { get; set; }

        public required CustomerModel Customer { get; set; }
    }
}
