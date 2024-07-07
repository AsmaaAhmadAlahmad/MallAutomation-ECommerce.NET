using AlhamraMall.Domains.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AlhamraMallApi.Repositories
{
    public interface IStoredProcedureRepository
    {
        Task<List<Product>> GetProductsByCommercialStoreIdAsync(Guid storeId);

        Task AddListOfCategoriesToCommercailStore(string categoriesIdsListAsString, Guid commercialStoreId);
    }
}
