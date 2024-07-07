
using AlhamraMall.Domains.Models;
using AlhamraMallApi.ApiModels;
using AlhamraMallApi.ApiModels.CartModels;
using AlhamraMallApi.ApiModels.OrderItemModels;
using AlhamraMallApi.Repositories;
using AlhamraMallApi.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace AlhamraMallApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "User", Policy = "ShouldBeUser")]
    [Authorize]

    public class CartsController : ControllerBase
    {
        private readonly IGenericRepository<Cart, CartForCreate, CartForUpdate> genericRepository;
        private readonly IGenericRepository<OrderItem, OrderItemForCreate, OrderItemForUpdate> genericRepositoryOrderItem;
        private readonly IGenericRepository<Customer, Customer, OrderItemForUpdate> genericRepositoryCustomer;

        public CartsController(IGenericRepository<Cart, CartForCreate, CartForUpdate> genericRepository,
                               IGenericRepository<OrderItem, OrderItemForCreate, OrderItemForUpdate> genericRepositoryOrderItem,
                               IGenericRepository<Customer, Customer, OrderItemForUpdate> genericRepositoryCustomer
                               )
                               
        {
            this.genericRepository = genericRepository;
            this.genericRepositoryOrderItem = genericRepositoryOrderItem;
            this.genericRepositoryCustomer = genericRepositoryCustomer;
        }






        [HttpGet("{cartId}", Name = "GetCart")]
        public async Task<ActionResult> GetCart(Guid cartId) // ايند بوينت جلب زبون واحد بواسطة الاي دي
        {
            var cart = await genericRepository.GetItemAsync(
                filterIdAndIsDeleted: c => c.IsDeleted != true && c.CartId == cartId); // الفلترة لجلب الزبون حسب الآي دي وأن يكون غبر محذوف

            if (cart == null)
                return NotFound(new ApiError
                {
                    ErrorCode = "CartNotFound",
                    ErrorMessage = "The cart does not exist"
                });

            return Ok(cart);
        }




        [HttpGet]
        public async Task<ActionResult> GetCarts() // ايند بوينت جلب كل الزبائن
        {
            var carts = await genericRepository.GetItemsAsync(filter: w => w.IsDeleted == false); // فلترة لجلب الزبائن الغير محذوفة فقط

            if (!carts.Any())
                return NotFound(new ApiError
                {
                    ErrorCode = "CartsNotFound",
                    ErrorMessage = "There are no carts available at the moment."
                });

            return Ok(carts);
        }






        [HttpDelete("{cartId}")]
        public async Task<ActionResult> DeleteCart(Guid cartId) // ايند بوينت حذف زبون
        {
            // جلب الزبون المُراد حذفه للتأكد من تواجده في قاعدة البيانات
            var cart = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(cartId);

            // الشرط الثاني للتأكد أن الزبون ليست محذوفة 
            if (cart == null || cart.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "CartNotFound",
                    ErrorMessage = "The cart does not exist"
                });
          
            // استدعاء دالة الحذف في حال كان الزبون ليس محذوف وموجود في القاعدة
            await genericRepository.DeleteItemAsync(cartId, cart);

            return NoContent();
        }





        //في الايند بوينت التالية حقنت الجينريك ريبوزيتوري الخاصة بالأوردر ايتيم  حتى استدعي الدالة
        //التي ستقوم بإضافة قائمة الاوردر ايتيمس الخاصة بهذه السلة التي سيتم اضافتها
        //ايند بوينت اضافة زبون
       [HttpPost]
        public async Task<ActionResult> AddCart(CartForCreate cartForCreate)
        {
            
            var cart = await genericRepository.AddItemAsync(cartForCreate);

         

            await genericRepository.save();

            return CreatedAtRoute("GetCart", new { cartId = cart.CartId }, cart);
        }




       












        [HttpPut("{cartId}")]
        public async Task<ActionResult> UpdateCart(Guid cartId, CartForUpdate cartForUpdate) // ايند بوينت تعديل منتج
        {
            // جلب الزبون المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
            var cart = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(cartId);

            // الشرط الثاني للتأكد أن الزبون ليست محذوفة  
            if (cart == null || cart.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "CartNotFound",
                    ErrorMessage = "The cart does not exist"
                });


            //  استدعاء دالة التعديل في حال كانت الزبون موجودة في قاعدة البيانات وليست محذوفة 
            await genericRepository.UpdateItemAsync(cartId, cartForUpdate);

            return NoContent();
        }








    
    }
}