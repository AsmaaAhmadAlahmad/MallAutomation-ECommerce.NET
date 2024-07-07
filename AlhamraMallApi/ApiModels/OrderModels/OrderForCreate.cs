using AlhamraMall.Domains.Models;
using AlhamraMallApi.ApiModels.OrderItemModels;
using System.ComponentModel.DataAnnotations;

namespace AlhamraMallApi.ApiModels.OrderModels
{
    public class OrderForCreate
    {
        [Required]
        public  Guid CustomerId { get; set; }

        [Required]
        public List<OrderItemForCreate> orderItemsForThisOrder { get; set; } = new List<OrderItemForCreate>();


    }
}