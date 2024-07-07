using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Models
{

    public class Product
    {
       
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; } 
        public string? Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public required decimal Price { get; set; }
        public string? FileName { get; set; } // اسم الصورة التي هي ستكون صورة المنتج
        public string? FilePath { get; set; } // مسار الصورة التي هي ستكون صورة المنتج
        public string? Description { get; set; } 
        public Guid CommercialStoreId { get; set; }
        public Product()
        {
            ProductId = Guid.NewGuid();
        }

        
    }
}
