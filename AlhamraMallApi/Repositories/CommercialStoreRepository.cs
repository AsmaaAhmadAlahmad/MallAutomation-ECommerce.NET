using AlhamraMall.Data;
using AlhamraMall.Domains.Helper;
using AlhamraMall.Domains.Models;
using AlhamraMallApi.ApiModels;
using AlhamraMallApi.ApiModels.CommercialStoreModels;
using Microsoft.EntityFrameworkCore;

namespace AlhamraMallApi.Repositories
{
    public class CommercialStoreRepository :GenericRepository<CommercialStore,CommercialStoreForCreate,CommercialStoreForUpdate>, ICommercialStoreRepository
    {
        private readonly AlhamraMallDbContext context1;

        public CommercialStoreRepository(AlhamraMallDbContext context1, HelperMapper<CommercialStore, CommercialStoreForCreate, CommercialStoreForUpdate> mapper) : base(context1, mapper)
        {
            this.context1 = context1;
        }

        public async Task<(List<CommercialStore>, PaginationMetaData)> FilterByCategoryAsync(Guid categoryId, int pageSize = 10, int pageNumber = 1)
        {
            // paginationMetaData حساب عدد العناصر الكلي لاجل ان يتم تمريره للمتغير 
            // ليقوم بمعرفة كم صفحة يتواجد لدينا 
            var totalItemCount = await context1.CommercialStores.Where(s => s.Categories
                                                                       .Any(c => c.Id == categoryId)).CountAsync(); 

            var paginationMetaData = new PaginationMetaData(totalItemCount, pageSize, pageNumber);
        

            //var (commercialStores, paginationMetaData) =

             var commercialStores = await base.GetItemsAsync( filter: s => s.IsDeleted == false
                                                      , includeProperties: "Categories"); // فلترة لجلب المحلات الغير محذوفة فقط

            var validCommercialStoresWithPagination = commercialStores.Where(s => s.Categories
                                                                       .Any(c => c.Id == categoryId)).Skip(pageSize * (pageNumber - 1)).Take(pageSize)
                                                       .ToList();

            return (validCommercialStoresWithPagination, paginationMetaData);
        }

     
   
    }
}
