namespace AlhamraMallApi.ApiModels.OrderItemModels
{
    public class OrderItemForUpdate
    {
        public Guid ProductId { get; set; }
        public required int Quantity { get; set; }
    }
}