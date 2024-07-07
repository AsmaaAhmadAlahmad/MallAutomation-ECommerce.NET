
using AlhamraMall.Domains.Models;
using AlhamraMallApi.ApiModels;
using AlhamraMallApi.ApiModels.CartModels;
using AlhamraMallApi.ApiModels.OrderItemModels;
using AlhamraMallApi.ApiModels.OrderModels;
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


    public class OrdersController : ControllerBase
    {
        private readonly IGenericRepository<Order, OrderForCreate, OrderForUpdate> genericRepository;
        private readonly IGenericRepository<OrderItem, OrderItemForCreate, OrderItemForUpdate> genericRepositoryOrderItem;
        private readonly IGenericRepository<Cart, CartForCreate, CartForUpdate> genericRepositoryCart;

        public OrdersController(IGenericRepository<Order, OrderForCreate, OrderForUpdate> genericRepository,
                               IGenericRepository<OrderItem, OrderItemForCreate, OrderItemForUpdate> genericRepositoryOrderItem,
                               IGenericRepository<Cart, CartForCreate, CartForUpdate> genericRepositoryCart
                                )
        {
            this.genericRepository = genericRepository;
            this.genericRepositoryOrderItem = genericRepositoryOrderItem;
            this.genericRepositoryCart = genericRepositoryCart;
        }






        [HttpGet("{orderId}", Name = "GetOrder")]
        public async Task<ActionResult> GetOrder(Guid orderId) // ايند بوينت جلب زبون واحد بواسطة الاي دي
        {
            var order = await genericRepository.GetItemAsync(
                filterIdAndIsDeleted: c => c.IsDeleted != true && c.OrderId == orderId); // الفلترة لجلب الزبون حسب الآي دي وأن يكون غبر محذوف

            if (order == null)
                return NotFound(new ApiError
                {
                    ErrorCode = "OrderNotFound",
                    ErrorMessage = "This order doesn't exist."
                });
            return Ok(order);
        }




        [HttpGet]
        public async Task<ActionResult> GetOrders() // ايند بوينت جلب كل الزبائن
        {
            var orders = await genericRepository.GetItemsAsync(filter: w => w.IsDeleted == false); // فلترة لجلب الزبائن الغير محذوفة فقط

            if (!orders.Any())
                return NotFound(new ApiError
                {
                    ErrorCode = "OrdersNotFound",
                    ErrorMessage = "There are no orders available at the moment."
                });
            return Ok(orders);
        }






        [HttpDelete("{orderId}")]
        public async Task<ActionResult> DeleteOrder(Guid orderId) // ايند بوينت حذف زبون
        {
            // جلب الزبون المُراد حذفه للتأكد من تواجده في قاعدة البيانات
            var order = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(orderId);

            // الشرط الثاني للتأكد أن الزبون ليست محذوفة 
            if (order == null || order.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "OrderNotFound",
                    ErrorMessage = "This order doesn't exist."
                });

            // استدعاء دالة الحذف في حال كان الزبون ليس محذوف وموجود في القاعدة
            await genericRepository.DeleteItemAsync(orderId, order);

            return NoContent();
        }






        // ايند بوينت اضافة زبون
        [HttpPost] 
        public async Task<ActionResult> AddOrder(OrderForCreate orderForCreate)
        {
            if (orderForCreate == null)
            return BadRequest(new ApiError
            {
                ErrorCode = "InvalidOrderData",
                ErrorMessage = "Invalid order data"
            });

           
            var ordrerThatCreated = await genericRepository.AddItemAsync(orderForCreate);

            var orderItems = await genericRepositoryOrderItem.AddRangeAsync(orderForCreate.orderItemsForThisOrder);

            ordrerThatCreated.orderItems = orderItems;

            // 'لها 'ترو  “IsOrdered”  جلب السلة التي يتم طلبها في هذا الطلب من اجل ان يتم وضع الخاصية 
            var cartThatOrdered =  await genericRepositoryCart.GetItemAsync(filterIdAndIsDeleted:c => c.IsDeleted == false 
                                                                            && c.CartId == orderItems.FirstOrDefault()!.CartId);

            // Set the “IsOrdered” property true for the cart that was ordered in this order
            cartThatOrdered!.IsOrdered = true;

            await genericRepository.save();

            return CreatedAtRoute("GetOrder", new { orderId = ordrerThatCreated.OrderId }, ordrerThatCreated);
          
        }








        [HttpPut("{orderId}")]
        public async Task<ActionResult> UpdateOrder(Guid orderId, OrderForUpdate orderForUpdate) // ايند بوينت تعديل منتج
        {
            // جلب الزبون المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
            var order = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(orderId);

            // الشرط الثاني للتأكد أن الزبون ليست محذوفة  
            if (order == null || order.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "OrderNotFound",
                    ErrorMessage = "This order doesn't exist."
                });

            //  استدعاء دالة التعديل في حال كانت الزبون موجودة في قاعدة البيانات وليست محذوفة 
            await genericRepository.UpdateItemAsync(orderId, orderForUpdate);

            return NoContent();
        }








    }
}