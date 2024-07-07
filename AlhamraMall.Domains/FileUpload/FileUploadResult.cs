using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.FileUpload
{
    public class FileUploadResult
    {
        
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public Guid ObjectId { get; set; }

    }
}
