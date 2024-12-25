using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using ResumeParser.API.ResumeConfigurations;
using Newtonsoft.Json;
using static ResumeParser.API.Model.Enums;
using UglyToad.PdfPig.Content;
using System.Collections.Generic;
using UglyToad.PdfPig;
using UglyToad.PdfPig.XObjects;
using System.Linq;

namespace ResumeParser.API.Model.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {

        [HttpPost]
        public ActionResult Post([FromForm] FileModel file)
        {
            string dir = Directory.GetCurrentDirectory();
            try
            {
                string path = System.IO.Path.Combine(dir, "wwwroot", file.FileName);
                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    file.FormFile.CopyTo(stream);
                }

                var resumeConfigFactory = new ResumeConfigurationFactory(path);
                var resumeConfig = resumeConfigFactory.GetConfiguraton(ResumeType.KariyerNet);
                var resumeModel = resumeConfig.Process();
                var jsonObject = JsonConvert.SerializeObject(resumeModel);

                return StatusCode(StatusCodes.Status201Created, jsonObject);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var path = @"/Users/borabilir/Projects/ResumeParser/ResumeParser.API/wwwroot/15102015 (4).pdf";

            string pdfFilePath = @"/Users/borabilir/Projects/ResumeParser/ResumeParser.API/wwwroot/15102015 (7).pdf";
            string targetFolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\target";

            try
            {
                using (PdfDocument pdfDocument = PdfDocument.Open(pdfFilePath))
                {
                    int imageCount = 0;

                    foreach (Page page in pdfDocument.GetPages())
                    {
                        var asd = page.GetWords().ToList();
                        List<XObjectImage> images = page.GetImages().Cast<XObjectImage>().ToList();
                        foreach (var Image in page.GetImages())
                        {
                            imageCount++;

                            string ImageType = String.Empty;

                            if (Image.TryGetPng(out var ByteArray))
                            {
                                ImageType = "png";
                                return File(ByteArray, "image/png");
                            }
                            else if (Image.TryGetBytes(out var ByteList))
                            {
                                ByteArray = ByteList.ToArray();
                                ImageType = "Most likely JPG";
                                return File(ByteArray, "image/jpg");
                            }
                            else
                            {
                                ByteArray = Image.RawBytes.ToArray();  //None of the filters are applied to the result. This can be an issue.
                                ImageType = "Unknown";
                                return File(ByteArray, "image/png");
                            }
                            // Process the ByteArray as an image. i.e. save it to a file, send it on to a downstream process.
                        }
                    }
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }

    //[HttpGet("{id}")]
    //    public ActionResult Get(int id)
    //    {
    //        int x = 1;
    //        var path = @"/Users/borabilir/Projects/ResumeParser/ResumeParser.API/wwwroot/15102015 (4).pdf";
    //        using (PdfDocument document = PdfDocument.Open(path))
    //        {
    //            foreach (Page page in document.GetPages())
    //            {

    //                  List<XObjectImage> images = page.GetImages().Cast<XObjectImage>().ToList();
    //                //foreach (var pdfImage in page.GetImages())
    //                //{
    //                //    if (x == id)
    //                //    {


    //                //        Byte[] b = (byte[])pdfImage.RawBytes;
    //                //        return File(b, "image/png");
    //                //    }
    //                //    x++;
    //                //}

    //            }
    //        }
    //        return null;
    //    }
}

