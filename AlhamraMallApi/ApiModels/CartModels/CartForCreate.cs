using AlhamraMallApi.ApiModels.OrderItemModels;

namespace AlhamraMallApi.ApiModels.CartModels
{
    public class CartForCreate
    {
        public string? CustomerEmail { get; set; }

        // لقد قمت بتعليق السطر التالي لانو خلص يعني يتم اضافة الاوردر ايتيمس لما بتم طلب السلة 
        //public List<OrderItemForCreate> orderItems { get; set; } = new List<OrderItemForCreate>();
       
    }
}