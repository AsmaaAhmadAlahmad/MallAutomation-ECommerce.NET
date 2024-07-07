using AlhamraMall.Domains.Models;
using AlhamraMallApi.ApiModels;
using AlhamraMallApi.ApiModels.CommercialStoreModels;

namespace AlhamraMallApi.Repositories
{
    public interface ICommercialStoreRepository:IGenericRepository<CommercialStore,CommercialStoreForCreate,CommercialStoreForUpdate>
    {
       Task<( List<CommercialStore>, PaginationMetaData)> FilterByCategoryAsync(Guid categoryId, int pageSize = 10, int pageNumber = 1);
    }
}
