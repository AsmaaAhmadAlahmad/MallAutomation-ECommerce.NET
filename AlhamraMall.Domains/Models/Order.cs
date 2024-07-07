using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Models
{
    // هذا الكلاس بعد أن يتم طلب السلة يعني ان يتم الدفع لشرائها 
    // عندها تصبح السلة طلب وبيجي دور هاد الكلاس 
    public class Order
    {

        public Guid OrderId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreateAt { get; set; } 
        public virtual ICollection<OrderItem> orderItems { get; set; }
        public virtual Customer? Customer { get; set; }
         public Guid CustomerId { get; set; } 

        public decimal Price => orderItems?.Sum(o=>o.Product?.Price * o.Quantity) ??  0;

        public Order()
        {
            orderItems = new List<OrderItem>();
            CreateAt = DateTime.Now;
            OrderId = Guid.NewGuid();
        }

    }
}
