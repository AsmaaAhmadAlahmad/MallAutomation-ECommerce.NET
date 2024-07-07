using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Models
{
    public class Cart
    {
         public Guid CartId { get; set; }
         public DateTime? CreateAt { get; set; }
         public bool IsOrdered { get; set; } 
         public  string? CustomerEmail { get; set; } 
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<OrderItem> orderItems { get; set; }
        public Cart()
        {
            CartId = Guid.NewGuid();
            orderItems = new List<OrderItem>();
            CreateAt = DateTime.UtcNow;
        }
    }
}
