using AlhamraMall.Domains.FileUpload;
using AlhamraMallApi.ApiModels.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Services
{
    public class FileUploadService
    {
        private readonly IWebHostEnvironment env;

        public FileUploadService(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public async Task<FileUploadResult> UploadFileAsync(IFormFile file, string folderName)
        {

           
            Guid objectId = Guid.NewGuid();

            // إنشاء اسم فريد للملف
            var fileName = objectId + Path.GetExtension(file.FileName);
            // الحصول على المسار الكامل للملف
            var filePath = Path.Combine(env.ContentRootPath, folderName, fileName);

            try
            {
                // التحقق من وجود المجلد وإنشائه إذا لم يكن موجودًا
                var uploadsFolder = Path.Combine(env.ContentRootPath, folderName);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // نسخ الملف إلى المسار المحدد
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // إعادة القيم المطلوبة
                var result = new FileUploadResult
                {
                    FileName = fileName,
                    FilePath = filePath,
                    ObjectId = objectId
                };

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("File upload failed", ex);
            }
        }
    }
}
