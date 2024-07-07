
using AlhamraMall.Domains.Models;
using AlhamraMall.Domains.Services;
using AlhamraMallApi.ApiModels.CategoryModels;
using AlhamraMallApi.ApiModels.ProductModels;
using AlhamraMallApi.Repositories;
using AlhamraMallApi.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AlhamraMallApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    //[Authorize(Roles = "User")]

    public class ProductsController : ControllerBase
    {
        public int maxPageSize = 5;
        private readonly IGenericRepository<Product, ProductForCreate, ProductForUpdate> genericRepository;

        private readonly IGenericRepository<Category, CategoryForCreate, CategoryForUpdate> genericRepositoryCategory;
        private readonly IWebHostEnvironment env;
        private readonly IStoredProcedureRepository storedProcedureRepository;
        private readonly FileUploadService fileUploadService;

        public ProductsController(IGenericRepository<Product, ProductForCreate, ProductForUpdate> genericRepository
                                  , IGenericRepository<Category, CategoryForCreate, CategoryForUpdate> genericRepositoryCategory
                                  ,  IWebHostEnvironment env
                                  , IStoredProcedureRepository storedProcedureRepository
                                  , FileUploadService fileUploadService)
        {
            this.genericRepository = genericRepository;
            this.genericRepositoryCategory = genericRepositoryCategory;
            this.env = env;
            this.storedProcedureRepository = storedProcedureRepository;
            this.fileUploadService = fileUploadService;
        }

        





        [HttpGet("{productId}", Name = "GetProduct")]
        public async Task<ActionResult> GetProduct(Guid productId) // ايند بوينت جلب منتج واحدة بواسطة الاي دي
       {
            var product = await genericRepository.GetItemAsync(
                filterIdAndIsDeleted: p => p.IsDeleted != true && p.ProductId == productId); // الفلترة لجلب المنتج حسب الآي دي وأن يكون غبر محذوف

            if (product == null)
                return NotFound();

            return Ok(product);
        }




        [HttpGet]
        public async Task<ActionResult> GetProducts(int pageSize=10, int pageNumber=1) // ايند بوينت جلب كل المنتجات
       {
            
            if (pageSize > 5)
                pageSize = maxPageSize;

            var (products,paginationMetaData) = await genericRepository.GetItemsAsyncAndPagination(pageNumber,
                                                                              pageSize,
                                                                              filter: w => w.IsDeleted == false); // فلترة لجلب المنتجات الغير محذوفة فقط

            if (!products.Any())
                return NotFound();

            Response.Headers.Append("X-Pagination",JsonSerializer.Serialize(paginationMetaData));

            return Ok(products);
        }



        [HttpGet("productsByCommercialStoreId/{commercialStore:guid}")]
        public async Task<ActionResult> GetProductsByCmmercialStoreId(Guid commercialStore) // ايند بوينت جلب كل المنتجات بواسطة رقم المحل
        {
            var products = await storedProcedureRepository.GetProductsByCommercialStoreIdAsync(commercialStore);

            if (!products.Any())
                return NotFound(new ApiError
                {
                    ErrorCode = "ProductsNotFound",
                    ErrorMessage = "There are no products availabl at the moment."
                });
            return Ok(products);
        }





        [HttpDelete("{productId}")]
        public async Task<ActionResult> DeleteProduct(Guid productId) // ايند بوينت حذف منتج
        {
            // جلب المنتج المُراد حذفه للتأكد من تواجده في قاعدة البيانات
            var product = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(productId);

            // الشرط الثاني للتأكد أن المنتج ليست محذوفة 
            if (product == null || product.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "ProductNotFound",
                    ErrorMessage = "This product doesn't exist."
                });
            // استدعاء دالة الحذف في حال كان المنتج ليس محذوف وموجود في القاعدة
            await genericRepository.DeleteItemAsync(productId, product);

            return NoContent();
        }






        // ايند بوينت اضافة منتج
        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm] ProductForCreate productForCreate)
        {
            if (productForCreate == null)
                return BadRequest(new ApiError {
                    ErrorCode = "InvalidProductData",
                    ErrorMessage = "Invalid product data"
                });

            if (productForCreate.File == null || productForCreate.File.Length == 0)
                return BadRequest(new ApiError
                {
                    ErrorCode = "FileNotSelected",
                    ErrorMessage = "File not selected."
                });

            if(productForCreate == null)
                return BadRequest(new ApiError
                {
                    ErrorCode = "InvalidProductData",
                    ErrorMessage = "Invalid product data."
                });

            var uploadedFileInfo =  await fileUploadService.UploadFileAsync(productForCreate.File
                                                                         , "Uploads/Products");


            

            // إضافة المنتج إلى المستودع وحفظ التغييرات
            Product productToAdd = await genericRepository.AddItemAsync(productForCreate);

                productToAdd.ProductId = uploadedFileInfo.ObjectId;
                productToAdd.FileName = uploadedFileInfo.FileName;
                productToAdd.FilePath = uploadedFileInfo.FilePath;

                await genericRepository.save();

                return Ok(new { Message = "Product created successfully" });
            
            
        }






















        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateProduct(Guid productId, ProductForUpdate productForUpdate) // ايند بوينت تعديل منتج
        {
            // جلب المنتج المُراد تعديلها للتأكد من تواجدها في قاعدة البيانات
            var product = await genericRepository.GetItemByIdForUpdateOrDeleteAsync(productId);

            // الشرط الثاني للتأكد أن المنتج ليست محذوفة  
            if (product == null || product.IsDeleted == true)
                return NotFound(new ApiError
                {
                    ErrorCode = "ProductNotFound",
                    ErrorMessage = "This product doesn't exist."
                });

            //  استدعاء دالة التعديل في حال كانت المنتج موجودة في قاعدة البيانات وليست محذوفة 
            await genericRepository.UpdateItemAsync(productId, productForUpdate);

            return NoContent();
        }


        [HttpGet("byCategory/{categoryId}")]
        public async Task<ActionResult> GetProductsByCatogeryId(Guid? categoryId)
        {
            if (categoryId == Guid.Empty || categoryId == null)
                return BadRequest(new ApiError
                {
                    ErrorCode = "InvalidCategoryId",
                    ErrorMessage = "Invalid categoryId. Please provide a valid category Id."
                });

            var products = await genericRepository.GetItemsAsync(filter: w => w.IsDeleted == false && w.CategoryId == categoryId);

            if (!products.Any())
                return NotFound(new ApiError
                {
                    ErrorCode = "ProductsNotFound",
                    ErrorMessage = "No products found for the specified category."
                });

            return Ok(products);
        }








     
    }
}