using AlhamraMall.Domains.Models;

namespace AlhamraMallApi.ApiModels.OrderItemModels
{
    public class OrderItemForCreate
    {
        public Guid ProductId { get; set; }
        public required int Quantity { get; set; } = 1;
        public Guid CartId { get; set; }

    }
}