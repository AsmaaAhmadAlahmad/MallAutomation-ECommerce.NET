using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Models
{

    // جدول المحلات التجارية 
    public class CommercialStore
    {
        public Guid Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Name { get; set; }
        public int DepartmentId { get; set; } // رقم الطابق الذي يتواجد فيه هذا المحل 
        public string Shop_owner_name { get; set; } // اسم صاحب  المحل 
        public string Shop_rent { get; set; } // ايجار المحل 
        public DateTime rental_date { get; set; } // تاريخ التأجير 
        public string? FileName { get; set; } // اسم الصورة التي هي ستكون صورة واجهة المحل
        public string? FilePath { get; set; } // مسار الصورة التي هي ستكون صورة واجهة المحل
        public List<Product> Products { get; set; } 

        public List<Category> Categories { get; set; }
        public CommercialStore() 
        {
            Id = new Guid();
            Categories = new List<Category>();
            rental_date = DateTime.UtcNow;
            Products= new List<Product>();
        }
    }
}
