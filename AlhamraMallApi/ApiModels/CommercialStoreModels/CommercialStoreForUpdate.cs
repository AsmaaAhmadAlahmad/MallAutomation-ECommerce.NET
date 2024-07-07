namespace AlhamraMallApi.ApiModels.CommercialStoreModels
{
    public class CommercialStoreForUpdate
    {
        public required string Name { get; set; }
        public required int DepartmentId { get; set; } // رقم القسم الذي يتواجد فيه هذا المحل 
        public required string Shop_owner_name { get; set; } // اسم صاحب  المحل 
        public required string Shop_rent { get; set; } // ايجار المحل 
    }
}