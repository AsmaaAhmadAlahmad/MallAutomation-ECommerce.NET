using AlhamraMallApi.ApiModels.CategoryModels;
using AlhamraMallApi.ApiModels.OrderItemModels;
using System.ComponentModel.DataAnnotations;

namespace AlhamraMallApi.ApiModels.CommercialStoreModels
{
    public class CommercialStoreForCreate
    {
        //وهو نفس الاسم الموجود في الكلاس الاصلي لذلك قمت بتغييره لغير اسم  Categories القائمة التالية كان اسمها 
        // مشان لما يتم عمل مابر من هاد النوع للنوع الاصلي يتم تجاهلها واساسا هي فقط قائمة ستأتي من الانكولار
        // تحتوي على عدة ارقام اصناف ليتم الاستفادة منها 
        [Required]
        public List<CategoriesForCreateForCommercialStore> CategoriesForCreateForCommercialStores { get; set; } = new List<CategoriesForCreateForCommercialStore>();
        public required string Name { get; set; }
        public required int DepartmentId { get; set; } //  هذا رقم الطابق الذي يتواجد فيه المحل 
        public required string Shop_owner_name { get; set; }  // اسم صاحب  المحل 
        public required string Shop_rent { get; set; } // ايجار المحل 

        [Required]
        public IFormFile File { get; set; }

    }

    public class CategoriesForCreateForCommercialStore
    {
        public Guid id { get; set; }

    }
}
  