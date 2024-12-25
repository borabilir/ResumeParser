using System;
using Microsoft.AspNetCore.Http;

namespace ResumeParser.API.Model
{
    public class FileModel
    {
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
