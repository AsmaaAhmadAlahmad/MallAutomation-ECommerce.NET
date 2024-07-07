
using AlhamraMall.Domains.Models;
using AlhamraMallApi.ApiModels;
using AlhamraMallApi.ApiModels.CustomerModels;
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

    public class CustomersController : ControllerBase
    {
        private readonly IGenericRepository<Customer, CustomerForCreate, CustomerForUpdate> genericRepository;

        public CustomersController(IGenericRepository<Customer, CustomerForCreate, CustomerForUpdate> genericRepository)
        {
            this.genericRepository = genericRepository;
        }




       

       

        [HttpGet("{customerId}", Name = "GetCustomer")]
        public async Task<ActionResult> GetCustomer(Guid customerId) // ايند بوينت جلب زبون واحد بواسطة الاي دي
        {
            var customer = await genericRepository.GetItemAsync(
                filterIdAndIsDeleted: c => c.IsDeleted != true && c.CustomerId == customerId); // الفلترة لجلب الزبون حسب الآي دي وأن يكون غبر محذوف

            if (customer == null)
                return NotFound(new ApiError
                {
                    ErrorCode = "CustomerNotFound",
                    ErrorMessage = "The customer does not exist"
                });

            return Ok(customer);
        }




        [HttpGet]
        public async Task<ActionResult> GetCustomers() // ايند بوينت جلب كل الزبائن
        {
            var customers = await genericRepository.GetItemsAsync(filter: w => w.IsDeleted == false); // فلترة لجلب الزبائن الغير محذوفة فقط

            if (!customers.Any())
                return NotFound(new ApiError
                {
                    ErrorCode = "CustomersNotFound",
                    ErrorMessage = "There are no customers available at the moment."
                });

            return Ok(customers);
        }






        [HttpDelete("{customerId}")]
        public async Task<ActionResult> DeleteCustomer(Guid customerId) // ايند بوينت حذف زبون
        {
            // جلب الزبون المُراد حذفه للتأكد من تواجده في قاعدة البيانات
            var customer = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(customerId);

            // الشرط الثاني للتأكد أن الزبون ليست محذوفة 
            if (customer == null || customer.IsDeleted == true)
                    return NotFound(new ApiError
                    {
                        ErrorCode = "CustomerNotFound",
                        ErrorMessage = "The customer does not exist"
                    });


            // استدعاء دالة الحذف في حال كان الزبون ليس محذوف وموجود في القاعدة
            await genericRepository.DeleteItemAsync(customerId, customer);

            return NoContent();
        }






        // ايند بوينت اضافة زبون
        [HttpPost] 
        public async Task<ActionResult> AddCustomer(CustomerForCreate customerForCreate)
        {
            if (customerForCreate == null)
                return BadRequest(new ApiError
                {
                    ErrorCode = "InvalidCustomerData",
                    ErrorMessage = "Invalid customer data"
                });

            // اضافة الزبون
            var customer = await genericRepository.AddItemAsync(customerForCreate);

            await genericRepository.save();

            return CreatedAtRoute("GetCustomer", new { customerId = customer.CustomerId }, customer);
        }








        [HttpPut("{customerId}")]
        public async Task<ActionResult> UpdateCustomer(Guid customerId, CustomerForUpdate customerForUpdate) // ايند بوينت تعديل منتج
        {
            // جلب الزبون المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
            var customer = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(customerId);

            // الشرط الثاني للتأكد أن الزبون ليست محذوفة  
            if (customer == null || customer.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "CustomerNotFound",
                    ErrorMessage = "The customer does not exist"
                });


            //  استدعاء دالة التعديل في حال كانت الزبون موجودة في قاعدة البيانات وليست محذوفة 
            await genericRepository.UpdateItemAsync(customerId, customerForUpdate);

            return NoContent();
        }








       
    }
}