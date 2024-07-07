using System.Xml.Schema;

namespace AlhamraMallApi.ApiModels
{
    public class PaginationMetaData
    {
        public int TotalItemCount { get; set; } // عدد العناصر الكلي 

        public int TotalPageCount { get; set; } // عدد الصفحات الكلي
        public int PageSize { get; set; } // حجم الصفحة
        public int CurrentPage { get; set; } // الصفحة الحالية
        public PaginationMetaData(int TotalItemCount,int PageSize,int CurrentPage)
        {
           TotalPageCount =(int) Math.Ceiling (TotalItemCount / (double) PageSize);
           this.TotalItemCount = TotalItemCount;
           this.CurrentPage = CurrentPage;
           this.PageSize = PageSize;

        }
    }
}
