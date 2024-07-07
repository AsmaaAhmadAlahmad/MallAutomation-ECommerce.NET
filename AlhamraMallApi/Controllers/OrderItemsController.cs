
using AlhamraMall.Domains.Models;
using AlhamraMallApi.ApiModels;
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

    public class OrderItemsController : ControllerBase
    {
        private readonly IGenericRepository<OrderItem, OrderItemForCreate, OrderItemForUpdate> genericRepository;

        public OrderItemsController(IGenericRepository<OrderItem, OrderItemForCreate, OrderItemForUpdate> genericRepository)
        {
            this.genericRepository = genericRepository;
        }






        [HttpGet("{orderItemId}", Name = "GetOrderItem")]
        public async Task<ActionResult> GetOrderItem(Guid orderItemId) // ايند بوينت جلب زبون واحد بواسطة الاي دي
        {
            var orderItem = await genericRepository.GetItemAsync(
                filterIdAndIsDeleted: c => c.IsDeleted != true && c.OrderItemId == orderItemId); // الفلترة لجلب الزبون حسب الآي دي وأن يكون غبر محذوف

            if (orderItem == null)
                return NotFound(new ApiError
                {
                    ErrorCode = "OrderItemNotFound",
                    ErrorMessage = "This orderItem doesn't exist."
                });

            return Ok(orderItem);
        }




        [HttpGet]
        public async Task<ActionResult> GetOrderItems() // ايند بوينت جلب كل الزبائن
        {
            var orderItems = await genericRepository.GetItemsAsync(filter: w => w.IsDeleted == false); // فلترة لجلب الزبائن الغير محذوفة فقط

            if (!orderItems.Any())
                return NotFound(new ApiError
                {
                    ErrorCode = "OrderItemsNotFound",
                    ErrorMessage = "There are no orderItems available at the moment."
                });

            return Ok(orderItems);
        }






        [HttpDelete("{orderItemId}")]
        public async Task<ActionResult> DeleteOrderItem(Guid orderItemId) // ايند بوينت حذف زبون
        {
            // جلب الزبون المُراد حذفه للتأكد من تواجده في قاعدة البيانات
            var orderItem = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(orderItemId);

            // الشرط الثاني للتأكد أن الزبون ليست محذوفة 
            if (orderItem == null || orderItem.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "OrderItemNotFound",
                    ErrorMessage = "This orderItem doesn't exist."
                });


            // استدعاء دالة الحذف في حال كان الزبون ليس محذوف وموجود في القاعدة
            await genericRepository.DeleteItemAsync(orderItemId, orderItem);

            return NoContent();
        }






        // ايند بوينت اضافة زبون
        [HttpPost] 
        public async Task<ActionResult> AddOrderItem(OrderItemForCreate orderItemForCreate)
        {
            if (orderItemForCreate == null)
                return BadRequest(new ApiError
                {
                    ErrorCode = "InvalidOrderItemData",
                    ErrorMessage = "Invalid orderItem data"
                });
            // اضافة الزبون
            var orderItem = await genericRepository.AddItemAsync(orderItemForCreate);

            await genericRepository.save();

            return CreatedAtRoute("GetOrderItem", new { orderItemId = orderItem.OrderItemId }, orderItem);
        }








        [HttpPut("{orderItemId}")]
        public async Task<ActionResult> UpdateOrderItem(Guid orderItemId, OrderItemForUpdate orderItemForUpdate) // ايند بوينت تعديل منتج
        {
            // جلب الزبون المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
            var orderItem = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(orderItemId);

            // الشرط الثاني للتأكد أن الزبون ليست محذوفة  
            if (orderItem == null || orderItem.IsDeleted == true)
            return NotFound(new ApiError
            {
                ErrorCode = "OrderItemNotFound",
                ErrorMessage = "This orderItem doesn't exist."
            });


            //  استدعاء دالة التعديل في حال كانت الزبون موجودة في قاعدة البيانات وليست محذوفة 
            await genericRepository.UpdateItemAsync(orderItemId, orderItemForUpdate);

            return NoContent();
        }








     
    }
}