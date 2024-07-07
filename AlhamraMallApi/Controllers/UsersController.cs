
using AlhamraMall.Domains.Entities;
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


    public class UsersController : ControllerBase
    {
        private readonly IGenericRepository<User, LoginModel, RegisterModel> genericRepository;

        public UsersController(IGenericRepository<User,LoginModel,RegisterModel> genericRepository)
        {
            this.genericRepository = genericRepository;
        }






        [HttpGet("{email}", Name = "GetUser")]
        public async Task<ActionResult> GetCart(string email) // ايند بوينت جلب زبون واحد بواسطة الاي دي
        {
            var user = await genericRepository.GetItemAsync(
                filterIdAndIsDeleted: c => c.IsDeleted != true && c.Email == email); // الفلترة لجلب الزبون حسب الآي دي وأن يكون غبر محذوف

            if (user == null)
                return NotFound(new ApiError
                {
                    ErrorCode = "UserNotFound",
                    ErrorMessage = "User not found"
                });

            return Ok(user);
        }




        //[HttpGet]
        //public async Task<ActionResult> GetCarts() // ايند بوينت جلب كل الزبائن
        //{
        //    var carts = await genericRepository.GetItemsAsync(filter: w => w.IsDeleted == false); // فلترة لجلب الزبائن الغير محذوفة فقط

        //    if (!carts.Any())
        //           return NotFound(new ApiError
        //{
        //    ErrorCode = "UsersNotFound",
        //    ErrorMessage = "There are no Users available at the moment  "
        //});

        //    return Ok(carts);
        //}






        //[HttpDelete("{cartId}")]
        //public async Task<ActionResult> DeleteCart(Guid cartId) // ايند بوينت حذف زبون
        //{
        //    // جلب الزبون المُراد حذفه للتأكد من تواجده في قاعدة البيانات
        //    var cart = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(cartId);

        //    // الشرط الثاني للتأكد أن الزبون ليست محذوفة 
        //    if (cart == null || cart.IsDeleted == true)
        ////           return NotFound(new ApiError
        //        {
        //            ErrorCode = "UserNotFound",
        //            ErrorMessage = "User not found"
        //        });

        //    // استدعاء دالة الحذف في حال كان الزبون ليس محذوف وموجود في القاعدة
        //    await genericRepository.DeleteItemAsync(cartId, cart);

        //    return NoContent();
        //}





        //    //في الايند بوينت التالية حقنت الجينريك ريبوزيتوري الخاصة بالأوردر ايتيم  حتى استدعي الدالة
        //    //التي ستقوم بإضافة قائمة الاوردر ايتيمس الخاصة بهذه السلة التي سيتم اضافتها
        //    //ايند بوينت اضافة زبون
        //   [HttpPost]
        //    public async Task<ActionResult> AddCart(CartForCreate cartForCreate)
        //    {
        //        if (cartForCreate is null)
        //            return BadRequest(new ApiError
                     //{
                     //    ErrorCode = "InvalidUserData",
                     //    ErrorMessage = "Invalid user data"
                     //});

        //        var cart1 = new CartForCreate
        //        {  // اضطررت لفعل هذا اللوجيك من اجل اخذ العناصر الموجودة في العنصر فقط 
        //           // من دون القائمة لانو خلال الاضافة عطاني مشاكل في المابر
        //           // orderForCreate والمشكلة هي وجود قائمة من النوع 
        //           // orderItem في هذا الكائن ووجود قائمة من النوع 
        //           // في النوع اللي رح يتم عمل مابر لنوعو وبالتالي هذا ماأدى
        //           // لظهور االمشكلة
        //            CustomerEmail = cartForCreate.CustomerEmail,
        //        };
        //        var cart = await genericRepository.AddItemAsync(cart1);

        //        var orderItems = await genericRepositoryOrderItem.AddRangeAsync(cartForCreate.orderItems);

        //        cart.orderItems = orderItems;

        //        await genericRepository.save();

        //        return CreatedAtRoute("GetCart", new { cartId = cart.CartId }, cart);
        //    }




        //    // الايند بوينت التالية هي لإضافة السلة 
        //    [HttpPost]
        //    [Route("Order")]
        //    public  IActionResult Create(CustomerModel customerModel )
        //    {
        //        //   if (createOrderModel.Customer is null)
        //        //   {
        //        //       ModelState.AddModelError("Customer", "You should pass the customer information");
        //        //   }

        //        //   // Customer user role
        //        //   if (createOrderModel.Customer?.Email?.Length <= 10)
        //        //   {
        //        //       ModelState.AddModelError("Email", "Email length should be greater than 10 characters");
        //        //   }

        //        //   if (!ModelState.IsValid)
        //        //   {
        //        //   }

        //        //   // اضافة الزبون الذي يفعل هذا الطلب 
        //        //   Customer customer = new Customer
        //        //   {
        //        //       City = createOrderModel.Customer.City,
        //        //       Country = createOrderModel.Customer.Country,
        //        //       PostalCode = createOrderModel.Customer.PostalCode,
        //        //       ShippingAdress = createOrderModel.Customer.ShippingAdress,
        //        //       Email = createOrderModel.Customer.Email,
        //        //       Name = createOrderModel.Customer.Name,
        //        //   };

        //        // await genericRepositoryCustomer.AddItemAsync(customer);

        //        //   var order = new Order { CustomerId = customer.CustomerId };

        //        //   if (createOrderModel.CartId == null || createOrderModel.CartId == Guid.Empty)
        //        //   {
        //        //       ModelState.AddModelError("Cart", "Missing or deleted shopping cart");
        //        //   }

        //        //   var cart =genericRepository.GetItemAsync(createOrderModel.CartId.Value);

        //        //   if (cart == null)
        //        //   {
        //        //       ModelState.AddModelError("Cart", "Cart has been deleted");

        //        //   }

        //        //   // اضافة العناصر التي في سلة التسوق الى جدول الاورد لانو خلص صرني طلبية والسلة تم طلبها
        //        //   foreach (var item in cart.orderItems)
        //        //   {
        //        //       order.orderItems.Add(item);
        //        //   }

        //        //  await genericRepositoryOrderItem.AddItemAsync(order);

        //        //await   genericRepositoryOrderItem.save();

        //        //   // تم طلب السلة 
        //        //   cart.IsOrdered = true;

        //        //   //cartRepository.Update(cart);

        //        //await   genericRepositoryOrderItem.save();

        //        //   stateRepository.Remove("CartId");

        //        //   stateRepository.Remove("NumberOfItems");

        //        //   return RedirectToAction("Thank you");
        //        return Ok();
        //    }












        //    [HttpPut("{cartId}")]
        //    public async Task<ActionResult> UpdateCart(Guid cartId, CartForUpdate cartForUpdate) // ايند بوينت تعديل منتج
        //    {
        //        // جلب الزبون المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
        //        var cart = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(cartId);

        //        // الشرط الثاني للتأكد أن الزبون ليست محذوفة  
        //        if (cart == null || cart.IsDeleted == true)
        ////           return NotFound(new ApiError
        //        {
        //            ErrorCode = "UserNotFound",
        //            ErrorMessage = "User not found"
        //        });

        //        //  استدعاء دالة التعديل في حال كانت الزبون موجودة في قاعدة البيانات وليست محذوفة 
        //        await genericRepository.UpdateItemAsync(cartId, cartForUpdate);

        //        return NoContent();
        //    }








        //    //    [HttpPatch("{carId}")]
        //    //    public async Task<ActionResult> UpdatePartiallyCar(int carId,
        //    //        JsonPatchDocument<CarForUpdate> patchDocument) // ايند بوينت تعديل بشكل جزئي
        //    //    {
        //    //        // جلب المنتج المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
        //    //        var car = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(carId); 

        //    //        // الشرط الثاني للتأكد أن المنتج ليست محذوفة
        //    //        if (car == null || car.IsDeleted==true)
        //    //            return NotFound();

        //    //        // استدعاء دالة التعديل الجزئي في حال كانت المنتج موجودة في قاعدة البيانات وليست محذوفة
        //    //        await genericRepository.UpdatePartiallyItemAsync(carId, patchDocument);

        //    //        return NoContent();
        //    //    }

        //    //}
    }
}