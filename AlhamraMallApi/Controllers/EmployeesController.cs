
using AlhamraMall.Domains.Models;
using AlhamraMallApi.ApiModels.EmployeeModels;
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


    public class EmployyesController : ControllerBase
    {
        private readonly IGenericRepository<Employee, EmployeeForCreate, EmployeeForUpdate> genericRepository;

        public EmployyesController(IGenericRepository<Employee, EmployeeForCreate, EmployeeForUpdate> genericRepository)
        {
            this.genericRepository = genericRepository;
        }






        [HttpGet("{employeeId}", Name = "GetEmployee")]
        public async Task<ActionResult> GetEmployee(Guid employeeId) // ايند بوينت جلب موظف واحد بواسطة الاي دي
        {
            var employee = await genericRepository.GetItemAsync(
                filterIdAndIsDeleted: e => e.IsDeleted != true && e.Id == employeeId); // الفلترة لجلب الموظف حسب الآي دي وأن يكون غبر محذوف

            if (employee == null)
                return NotFound(new ApiError
                {
                    ErrorCode = "EmployeeNotFound",
                    ErrorMessage = "The employee does not exist"
                });

            return Ok(employee);
        }




        [HttpGet]
        public async Task<ActionResult> GetEmployees() // ايند بوينت جلب كل الموظفين
        {
            var employees = await genericRepository.GetItemsAsync(filter: w => w.IsDeleted == false); // فلترة لجلب الموظفين الغير محذوفة فقط

            if (!employees.Any())
                return NotFound(new ApiError
                {
                    ErrorCode = "EmployeesNotFound",
                    ErrorMessage = "There are no employees available at the moment."
                });

            return Ok(employees);
        }






        [HttpDelete("{employeeId}")]
        public async Task<ActionResult> DeleteEmployee(Guid employeeId) // ايند بوينت حذف موظف
        {
            // جلب الموظف المُراد حذفه للتأكد من تواجده في قاعدة البيانات
            var employee = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(employeeId);

            // الشرط الثاني للتأكد أن الموظف ليست محذوفة 
            if (employee == null || employee.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "EmployeeNotFound",
                    ErrorMessage = "The employee does not exist"
                });

            // استدعاء دالة الحذف في حال كان الموظف ليس محذوف وموجود في القاعدة
            await genericRepository.DeleteItemAsync(employeeId, employee);

            return NoContent();
        }






        // ايند بوينت اضافة موظف
        [HttpPost]
        public async Task<ActionResult> AddEmployee(EmployeeForCreate employeeForCreate)
        {
            if (employeeForCreate == null)
            return BadRequest(new ApiError
            {
                ErrorCode = "InvalidEmployeeData",
                ErrorMessage = "Invalid employee data"
            });
            // اضافة الموظف
            var employee = await genericRepository.AddItemAsync(employeeForCreate);

            await genericRepository.save();

            return CreatedAtRoute("GetEmployee", new { employeeId = employee.Id }, employee);
        }








        [HttpPut("{employeeId}")]
        public async Task<ActionResult> UpdateEmployee(Guid employeeId, EmployeeForUpdate employeeForUpdate) // ايند بوينت تعديل منتج
        {
            // جلب الموظف المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
            var employee = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(employeeId);

            // الشرط الثاني للتأكد أن الموظف ليست محذوفة  
            if (employee == null || employee.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "EmployeeNotFound",
                    ErrorMessage = "The employee does not exist"
                });

            //  استدعاء دالة التعديل في حال كانت الموظف موجودة في قاعدة البيانات وليست محذوفة 
            await genericRepository.UpdateItemAsync(employeeId, employeeForUpdate);

            return NoContent();
        }








    }
}