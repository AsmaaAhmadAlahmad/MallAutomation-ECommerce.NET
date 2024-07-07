using AlhamraMall.Domains.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AlhamraMallApi.Repositories
{
    public class StoredProcedureRepository : IStoredProcedureRepository
    {
        private readonly IConfiguration _configuration;

        private readonly string connectionString;
        public StoredProcedureRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("AlhamraMallContext");
        }


        // تم استدعاء الستوريد بروسيجر في البلوك السفلي من اجل ان تقوم بإضافة قائمة المفاتيح 
        // مع رقم المحل الى الجدول الوسيط 
        // يعني بتضيف رقم المحل ثم رقم الصنف في حقل رقم الصنف
        // وتكرر هكذا حتى تنتهي قائمة المفاتيح وبالتالي تمت اضافة قائمة الاصناف التي تتواجد في هذا المحل
        public async Task AddListOfCategoriesToCommercailStore(string categoriesIdsListAsString, Guid commercialStoreId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync(); // فتح الاتصال بقاعدة البيانات بشكل غير متزامن

                using (SqlCommand command = new SqlCommand("AddListOfCategoriesToCommercialStore", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // إضافة المعاملات لإجراء التخزين المخزن
                    command.Parameters.Add(new SqlParameter("@ListIds", SqlDbType.VarChar)
                    {
                        Value = categoriesIdsListAsString
                    });
                    command.Parameters.Add(new SqlParameter("@CommercialStoreId", SqlDbType.UniqueIdentifier)
                    {
                        Value = commercialStoreId
                    });

                    // تنفيذ إجراء التخزين المخزن بشكل غير متزامن
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    

    public async Task<List<Product>> GetProductsByCommercialStoreIdAsync(Guid storeId)
        {
            // جلب الكونيكشن سترينغ    
            var products = new List<Product>();

            using (var connection = new SqlConnection(connectionString))
            {
                
                using (var command = new SqlCommand("getProductsByCommercailStoreId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // إضافة البارامتر اللازم للإجراء المخزن
                    command.Parameters.Add(new SqlParameter("@commercailStoreId", storeId));

                    await connection.OpenAsync();

                    // تنفيذ الأمر وقراءة النتائج
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // قراءة النتائج من القارئ
                        while (await reader.ReadAsync())
                        {
                            // إنشاء كائن منتج جديد وتعبئة خصائصه من النتائج
                            var product = new Product
                            {
                                ProductId = reader.GetGuid(reader.GetOrdinal("ProductId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                FileName = reader.GetString(reader.GetOrdinal("FileName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                CategoryId = reader.GetGuid(reader.GetOrdinal("CategoryId"))
                            };

                            // إضافة المنتج إلى قائمة المنتجات
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }



    }
}

