using AlhamraMall.Domains.Models;
using AlhamraMall.Domains.Services;
using AlhamraMallApi.ApiModels.CommercialStoreModels;
using AlhamraMallApi.ApiModels.ProductModels;
using AlhamraMallApi.Repositories;
using AlhamraMallApi.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;

namespace AlhamraMallApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "User", Policy = "ShouldBeUser")]


    public class CommercialStoresController : ControllerBase
    {
        private readonly IGenericRepository<CommercialStore, CommercialStoreForCreate, CommercialStoreForUpdate> genericRepository;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;
        private readonly ICommercialStoreRepository commercialStoreRepository;
        private readonly FileUploadService fileUploadService;
        private readonly IStoredProcedureRepository storedProcedureRepository;

        public CommercialStoresController(IGenericRepository<CommercialStore, CommercialStoreForCreate, CommercialStoreForUpdate> genericRepository
                                          , IWebHostEnvironment env
                                          ,IConfiguration configuration,
                                           ICommercialStoreRepository commercialStoreRepository
                                          , FileUploadService fileUploadService
                                          , IStoredProcedureRepository storedProcedureRepository)
        {
            this.genericRepository = genericRepository;
            this.env = env;
            this.configuration = configuration;
            this.commercialStoreRepository = commercialStoreRepository;
            this.fileUploadService = fileUploadService;
            this.storedProcedureRepository = storedProcedureRepository;
        }

        [Route("by-category/{categoryId}")]
        [HttpGet]
        public async Task<IActionResult> FilterByCategory(Guid categoryId, int pageSize = 10, int pageNumber = 1)
        {
           if (categoryId == Guid.Empty)
                return BadRequest();
          
            var validCommercialStoresWithPagination = await commercialStoreRepository.FilterByCategoryAsync(categoryId, pageSize, pageNumber);

            if (!validCommercialStoresWithPagination.Item1.Any())
                return NotFound();

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(validCommercialStoresWithPagination.Item2));

            return Ok(validCommercialStoresWithPagination.Item1);
        }





        [HttpGet("{commercialStoreId}", Name = "GetCommercialStore")]
        public async Task<ActionResult> GetCommercialStore(Guid commercialStoreId) // ايند بوينت جلب محل واحد بواسطة الاي دي
        {
            var commercialStore = await genericRepository.GetItemAsync(
                filterIdAndIsDeleted: s => s.IsDeleted != true && s.Id == commercialStoreId
                ,includeProperties: "Categories"); // الفلترة لجلب المحل حسب الآي دي وأن يكون غبر محذوف

            if (commercialStore == null)
                return NotFound();

            return Ok(commercialStore);
        }




        [HttpGet]
        public async Task<ActionResult<List<CommercialStore>>> GetCommercialStores() // ايند بوينت جلب كل المحلات
        {
            var commercialStores = await genericRepository.GetItemsAsync(filter: s => s.IsDeleted == false,
                                                                         includeProperties: "Categories"); // فلترة لجلب المحلات الغير محذوفة فقط

            if (!commercialStores.Any())
                return NotFound();

            return Ok(commercialStores);
        }






        [HttpDelete("{commercialStoreId}")]
        public async Task<ActionResult> DeleteCategory(Guid commercialStoreId) // ايند بوينت حذف محل
        {
            // جلب المحل المُراد حذفه للتأكد من تواجده في قاعدة البيانات
            var commercialStore = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(commercialStoreId);

            // الشرط الثاني للتأكد أن المحل ليس محذوف 
            if (commercialStore == null || commercialStore.IsDeleted == true)
                return NotFound();

            // استدعاء دالة الحذف في حال كان المحل ليس محذوف وموجود في القاعدة
            await genericRepository.DeleteItemAsync(commercialStoreId, commercialStore);

            return NoContent();
        }




        // ايند بوينت اضافة محل
        [HttpPost] 
        public async Task<ActionResult> AddCommercialStore(CommercialStoreForCreate commercialStoreForCreate)
        {

            if (commercialStoreForCreate is null)
                return BadRequest(new ApiError
                {
                    ErrorCode = "InvalidCommercialStoreData",
                    ErrorMessage = "Invalid commercialStore data"
                });

            if (commercialStoreForCreate.File == null || commercialStoreForCreate.File.Length == 0)
                return BadRequest(new ApiError
                {
                    ErrorCode = "FileNotSelected",
                    ErrorMessage = "File not selected"
                });


            var uploadedFileInfo = await fileUploadService.UploadFileAsync(commercialStoreForCreate.File
                                                                       , "Uploads/CommercialStores");

           

            // اضافة المحل
            var commercialStore = await genericRepository.AddItemAsync(commercialStoreForCreate);
                commercialStore.Id =       uploadedFileInfo.ObjectId;
                commercialStore.FilePath = uploadedFileInfo.FilePath;
                commercialStore.FileName = uploadedFileInfo.FileName;
                await genericRepository.save();

                // في السطر التالي تم فيه استخراج المفاتيح من قائمة الاصناف التي اتت من الفرونت ايند 
                // من اجل ان يتم اضافة هذه المفاتيح الى الجدول الوسيط على انها تابعة للمحل هذا الذي تمت اضافته
                var categoriesIdsList = commercialStoreForCreate.CategoriesForCreateForCommercialStores.Select(c => c.id).ToList();

                // تحويل قائمة المفاتيح الى سترينغ لان الستوريد بروسيجر تطلبها كسترينغ 
                string categoriesIdsListAsString = ConvertGuidListToString(categoriesIdsList);
                
                string connectionString = configuration.GetConnectionString("AlhamraMallContext");

            
                await storedProcedureRepository.AddListOfCategoriesToCommercailStore(categoriesIdsListAsString
                                                                                , commercialStore.Id);
               
                    return CreatedAtRoute("GetCommercialStore", new { commercialStoreId = commercialStore.Id }, commercialStore);
            
        }








        [HttpPut("{commercialStoreId}")]
        public async Task<ActionResult> UpdateCommercialStore(Guid commercialStoreId, CommercialStoreForUpdate commercialStoreForUpdate) // ايند بوينت تعديل منتج
        {
            // جلب المحل المُراد تعديه للتأكد من تواجده في قاعدة البيانات
            var commercialStore = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(commercialStoreId);

            // الشرط الثاني للتأكد أن المحل ليس محذوف  
            if (commercialStore == null || commercialStore.IsDeleted == true)
                return NotFound();

            //  استدعاء دالة التعديل في حال كان المحل موجود في قاعدة البيانات وليست محذوف 
            await genericRepository.UpdateItemAsync(commercialStoreId, commercialStoreForUpdate);

            return NoContent();
        }




        public static string ConvertGuidListToString(List<Guid> guidList)
        {
            return string.Join(",", guidList.Select(g => g.ToString()));
        }



       
    }
}




