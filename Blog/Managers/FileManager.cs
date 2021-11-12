using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Managers
{
    public class FileManager
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileManager(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetUniqueNameAndSavePhotoToDisk(IFormFile pictureFile)
        {
            string uniqueFileName = default;

            if (pictureFile is not null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + pictureFile.FileName;

                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    pictureFile.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }


        public void RemoveImageFromDisk(string imageName)
        {
            if (!string.IsNullOrEmpty(imageName))
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                string filePath = Path.Combine(uploadsFolder, imageName);
                System.IO.File.Delete(filePath);
            }
        }



    }
}
