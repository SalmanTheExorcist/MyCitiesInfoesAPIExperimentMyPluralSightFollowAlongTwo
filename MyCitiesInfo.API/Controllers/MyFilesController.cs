using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace MyCitiesInfo.API.Controllers
{
    [Route("api/v{version:apiVersion}/myfiles")]
    [ApiController]
    [Authorize]
    public class MyFilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public MyFilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                    ?? throw new System.ArgumentException(nameof(fileExtensionContentTypeProvider));

        }

        //-----------------------------------------------------------

        [HttpGet("{myfileId:int}")]
        [ApiVersion(0.1, Deprecated = true)]
        public ActionResult GetSingleMyFile(int myfileId)

        {
            //look for the file by Id 
            //--this is for demo-purposes
            var myFilePath = "getting-started-with-rest-slides.pdf";

            //now check file exist
            if (!System.IO.File.Exists(myFilePath))
            {
                return NotFound();
            }


            if(!_fileExtensionContentTypeProvider.TryGetContentType(
                                    myFilePath,out var contentType))
            {
                contentType = "application/octet-stream";
            }


            var myFileBytes = System.IO.File.ReadAllBytes(myFilePath);
            return File(myFileBytes, contentType, Path.GetFileName(myFilePath));



        }

        //-----------------------------------------------------------
        //--Create/Uploading a file

        [HttpPost]
        public async Task<ActionResult> CreateMyFile(IFormFile myFile)
        {
            //--Validate the input file:
            //---limitting the file size
            //---only accepting PDF files by checking "Content-Type".
            //--
            if (myFile.Length == 0 || myFile.Length > 20971520
                || myFile.ContentType != "application/pdf")
            {
                return BadRequest("No File or Invalid file has been submitted");
            }

            //--Create  the file path. (Note): avoid using "file.FileName" because attacker 
            //----may use malicious filenames. 
            //--
            var myFilePath = Path.Combine(Directory.GetCurrentDirectory(),
                                          $"uploaded_file_{Guid.NewGuid()}.pdf");

            //--finally initialze filestream:
            using (var myFileStream = new FileStream(myFilePath, FileMode.Create))
            {
                //--now use the filestream to upload the file from client
                await myFile.CopyToAsync(myFileStream);
            }

            return Ok("Your file has been uploaded successfully!");


        }//End-HttpPost


        //-----------------------------------------------------------

    }//--End-Class
}//--End-Namespace
