using AlhamraMallApi.ApiModels;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;
namespace AlhamraMallApi.Repositories
{
    //  من أجل الكلاس الأصلي T
    // ApiModels  من أجل الكلاس الذي يُستخدم للإضافة والمتواجد في مجلد  B
    // ApiModels  من أجل الكلاس الذي يُستخدم للتعديل والمتواجد في مجلد C
    public interface IGenericRepository<T, B, C> where T : class where B : class where C : class
    {
        // في الدالة التالية يتم الفلترة عن العنصر المطلوب بواسطة الآي دي داخل الفلتر 
        // حيث قمت بتمرير شرط خلال اقواس دالة الجلب في داخل كل متحكم ليتم جلب العنصر 
        // الذي مررنا الآي دي الذي يخصه يعني لم يتبقى حاجة لتمرير الآي دي كبارامتر في الدالة
        Task<T?> GetItemAsync(
            //  int id, 
            Expression<Func<T, bool>> filterIdAndIsDeleted = null // هذا البارامتر لوضع فلتر على البيانات
            , string includeProperties = ""); // هذا البارامتر لتضمين خصائص القوائم في الكلاسات

        Task<IEnumerable<T>> GetItemsAsync(
        // int? pageNumber // لقد وضعت هذا البارمتر اختياري لأنه ليس كل الجداول اريد عمل بيجينشن لها 
        //                 // وهذا يعني انه في متحكم ما لن يتم تمرير هذا البارامتر يعني لن يتم الحاجة اليه
        //                 // والبارامتر السفلي ايضا نفس الكلام 
        //, int? pageSize // هذا البارامتر لتضمين خصائص القوائم في الكلاسات
        Expression<Func<T, bool>> filter = null // هذا البارامتر لوضع فلتر على البيانات
        , string includeProperties = "");

        Task DeleteItemAsync(Guid id, T item); // دالة حذف كائن 

        Task UpdateItemAsync(Guid id, C item); // دالة تعديل كائن

        Task<T> AddItemAsync(B item);

        Task<List<T>> AddRangeAsync(List<B> bs); // دالة اضافة قائمة لكائن ما


        Task UpdatePartiallyItemAsync(Guid id, JsonPatchDocument<C> patchDocument); // دالة تعديل جزئي

        Task save(); // دالة للحفظ في قاعدة البيانات

       


        Task<T> GetItemByIdForUpdateOrDeleteAsync(Guid id); // الدالة التالية فقط من اجل التأكد من تواجد العنصر قبل حذفه أو تعديله
                                                            // لأن العمليات على العناصر المحذوفة يجب أن تكون ممنوعة

        Task<(IEnumerable<T>,PaginationMetaData)> GetItemsAsyncAndPagination(
         int pageNumber 
       , int pageSize 
       , Expression<Func<T, bool>> filter = null // هذا البارامتر لوضع فلتر على البيانات
      , string includeProperties = "");
    }
}