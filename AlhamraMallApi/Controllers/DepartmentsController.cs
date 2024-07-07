
//using AlhamraMall.Domains.Models;
//using AlhamraMallApi.ApiModels;
//using AlhamraMallApi.ApiModels.DepartmentModels;
//using AlhamraMallApi.Repositories;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.JsonPatch;
//using Microsoft.AspNetCore.Mvc;

//namespace AlhamraMallApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    //[Authorize(Roles = "User", Policy = "ShouldBeUser")]


//    public class DepartmentsController : ControllerBase
//    {
//        private readonly IGenericRepository<Department, DepartmentForCreate, DepartmentForUpdate> genericRepository;

//        public DepartmentsController(IGenericRepository<Department, DepartmentForCreate, DepartmentForUpdate> genericRepository)
//        {
//            this.genericRepository = genericRepository;
//        }






//        [HttpGet("{departmentId}", Name = "GetDepartment")]
//        public async Task<ActionResult> GetDepartment(Guid departmentId) // ايند بوينت جلب قسم واحد بواسطة الاي دي
//        {
//            var department = await genericRepository.GetItemAsync(
//                filterIdAndIsDeleted: d => d.IsDeleted != true && d.Id == departmentId); // الفلترة لجلب القسم حسب الآي دي وأن يكون غبر محذوف

//            if (department == null)
//                return NotFound();

//            return Ok(department);
//        }




//        [HttpGet]
//        public async Task<ActionResult> GetDepartments() // ايند بوينت جلب كل الاقسام
//        {
//            var departments = await genericRepository.GetItemsAsync(filter: d => d.IsDeleted == false); // فلترة لجلب الاقسام الغير محذوفة فقط

//            if (!departments.Any())
//                return NotFound();

//            return Ok(departments);
//        }






//        [HttpDelete("{departmentId}")]
//        public async Task<ActionResult> DeleteDepartment(Guid departmentId) // ايند بوينت حذف قسم
//        {
//            // جلب القسم المُراد حذفه للتأكد من تواجده في قاعدة البيانات
//            var department = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(departmentId);

//            // الشرط الثاني للتأكد أن القسم ليست محذوفة 
//            if (department == null || department.IsDeleted == true)
//                return NotFound();

//            // استدعاء دالة الحذف في حال كان القسم ليس محذوف وموجود في القاعدة
//            await genericRepository.DeleteItemAsync(departmentId, department);

//            return NoContent();
//        }






//        // ايند بوينت اضافة قسم
//        [HttpPost]
//        public async Task<ActionResult> AddDepartment(DepartmentForCreate departmentForCreate)
//        {
//            // اضافة القسم
//            var department = await genericRepository.AddItemAsync(departmentForCreate);

//            await genericRepository.save();

//            return CreatedAtRoute("GetDepartment", new { departmentId = department.Id }, department);
//        }








//        [HttpPut("{departmentId}")]
//        public async Task<ActionResult> UpdateDepartment(Guid departmentId, DepartmentForUpdate departmentForUpdate) // ايند بوينت تعديل منتج
//        {
//            // جلب القسم المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
//            var department = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(departmentId);

//            // الشرط الثاني للتأكد أن القسم ليست محذوفة  
//            if (department == null || department.IsDeleted == true)
//                return NotFound();

//            //  استدعاء دالة التعديل في حال كانت القسم موجودة في قاعدة البيانات وليست محذوفة 
//            await genericRepository.UpdateItemAsync(departmentId, departmentForUpdate);

//            return NoContent();
//        }








//        //    [HttpPatch("{carId}")]
//        //    public async Task<ActionResult> UpdatePartiallyCar(int carId,
//        //        JsonPatchDocument<CarForUpdate> patchDocument) // ايند بوينت تعديل بشكل جزئي
//        //    {
//        //        // جلب المنتج المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
//        //        var car = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(carId); 

//        //        // الشرط الثاني للتأكد أن المنتج ليست محذوفة
//        //        if (car == null || car.IsDeleted==true)
//        //            return NotFound();

//        //        // استدعاء دالة التعديل الجزئي في حال كانت المنتج موجودة في قاعدة البيانات وليست محذوفة
//        //        await genericRepository.UpdatePartiallyItemAsync(carId, patchDocument);

//        //        return NoContent();
//        //    }

//        //}
//    }
//}