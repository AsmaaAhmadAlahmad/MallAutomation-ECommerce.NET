
using AlhamraMall.Domains.Models;
using AlhamraMallApi.ApiModels.CategoryModels;
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
    //[Authorize]


    public class CategoriesController : ControllerBase
    {
        private readonly IGenericRepository<Category, CategoryForCreate, CategoryForUpdate> genericRepository;

        public CategoriesController(IGenericRepository<Category, CategoryForCreate, CategoryForUpdate> genericRepository)
        {
            this.genericRepository = genericRepository;
        }






        [HttpGet("{categoryId}", Name = "GetCategory")]
        public async Task<ActionResult> GetCategory(Guid categoryId) // ايند بوينت جلب صنف واحد بواسطة الاي دي
        {
            var category = await genericRepository.GetItemAsync(
                filterIdAndIsDeleted: c => c.IsDeleted != true && c.Id == categoryId); // الفلترة لجلب الصنف حسب الآي دي وأن يكون غبر محذوف

            if (category == null)
                return NotFound(new ApiError
                {
                    ErrorCode = "CategoryNotFound",
                    ErrorMessage = "The category does not exist"
                });

            return Ok(category);
        }




        [HttpGet]
        public async Task<ActionResult> GetCategories() // ايند بوينت جلب كل الاصناف
        {
            var categories = await genericRepository.GetItemsAsync(filter: w => w.IsDeleted == false); // فلترة لجلب الاصناف الغير محذوفة فقط

            if (!categories.Any())
                return NotFound( new ApiError
                {
                    ErrorCode = "CategoriesNotFound",
                    ErrorMessage = "There are no categories available at the moment."
                });

            return Ok(categories);
        }






        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategory(Guid categoryId) // ايند بوينت حذف صنف
        {
            // جلب الصنف المُراد حذفه للتأكد من تواجده في قاعدة البيانات
            var category = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(categoryId);

            // الشرط الثاني للتأكد أن الصنف ليست محذوفة 
            if (category == null || category.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "CategoryNotFound",
                    ErrorMessage = "This category is not found."
                });

            // استدعاء دالة الحذف في حال كان الصنف ليس محذوف وموجود في القاعدة
            await genericRepository.DeleteItemAsync(categoryId, category);

            return NoContent();
        }






        // ايند بوينت اضافة صنف
        [HttpPost] 
        public async Task<ActionResult> AddCategory(CategoryForCreate categoryForCreate)
        {
            if (categoryForCreate == null)
                return BadRequest(new ApiError
                {
                    ErrorCode = "InvalidCategoryData",
                    ErrorMessage = "Invalid category data"
                });
            // اضافة الصنف
            var category = await genericRepository.AddItemAsync(categoryForCreate);

            await genericRepository.save();

            return CreatedAtRoute("GetCategory", new { categoryId = category.Id }, category);
        }








        [HttpPut("{categoryId}")]
        public async Task<ActionResult> UpdateCategory(Guid categoryId, CategoryForUpdate categoryForUpdate) // ايند بوينت تعديل منتج
        {
            // جلب الصنف المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
            var category = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(categoryId);

            // الشرط الثاني للتأكد أن الصنف ليس محذوف  
            if (category == null || category.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "CategoryNotFound",
                    ErrorMessage = "This category is not found."
                });

            //  استدعاء دالة التعديل في حال كانت الصنف موجودة في قاعدة البيانات وليس محذوف 
            await genericRepository.UpdateItemAsync(categoryId, categoryForUpdate);

            return NoContent();
        }








        
    }
}