using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }

        public Guid ProductId { get; set; }
        public required int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual Product? Product { get; set; }

        public Guid CartId { get; set; }


        public OrderItem()
        {
            OrderItemId = Guid.NewGuid();
        }

    }
}
