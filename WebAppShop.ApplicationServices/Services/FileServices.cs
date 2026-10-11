using Microsoft.Extensions.Hosting;
using WebAppShop.Core.Domain;
using WebAppShop.Core.Dto;
using WebAppShop.Core.ServiceInterface;
using WebAppShop.Data;


namespace WebAppShop.ApplicationServices.Services
{
    public class FileServices : IFileServices
    {
        private readonly WebAppShopContext _context;
        private readonly IHostEnvironment _webHost;

        public FileServices
            (
                WebAppShopContext context,
                IHostEnvironment webHost
            )
        {
            _context = context;
            _webHost = webHost;
        }


        public void FilesToApi(SpaceshipDto dto, Spaceship domain)
        {
            // Kontrollime, kas failid on lisatud
            if (dto.Files != null && dto.Files.Count > 0)
            {
                string uploadsFolder = Path.Combine(
                    _webHost.ContentRootPath,
                    "wwwroot",
                    "multipleFileUpload"
                );

                // Loome kausta, kui seda veel ei ole
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var file in dto.Files)
                {
                    // Loome failile unikaalse nime
                    string uniqueFileName = Guid.NewGuid().ToString()
                        + "_" + Path.GetFileName(file.FileName);

                    string filePath = Path.Combine(
                        uploadsFolder,
                        uniqueFileName
                    );

                    // Salvestame faili
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Salvestame faili tee andmebaasi
                    FileToApi image = new FileToApi
                    {
                        Id = Guid.NewGuid(),
                        ExistingFilePath = "/multipleFileUpload/" + uniqueFileName,
                        SpaceshipId = domain.Id
                    };

                    _context.FileToApis.Add(image);
                }

                _context.SaveChanges();
            }
        }


        public void DeleteSpaceshipFiles(Guid spaceshipId)
        {
            var images = _context.FileToApis
                .Where(x => x.SpaceshipId == spaceshipId)
                .ToList();

            foreach (var image in images)
            {
                if (!string.IsNullOrEmpty(image.ExistingFilePath))
                {
                    string filePath = Path.Combine(
                        _webHost.ContentRootPath,
                        "wwwroot",
                        "multipleFileUpload",
                        Path.GetFileName(image.ExistingFilePath)
                    );

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            _context.FileToApis.RemoveRange(images);
            _context.SaveChanges();
        }


        public void DeleteSelectedFiles(List<Guid> imageIds)
        {
            var images = _context.FileToApis
                .Where(x => imageIds.Contains(x.Id))
                .ToList();

            foreach (var image in images)
            {
                if (!string.IsNullOrEmpty(image.ExistingFilePath))
                {
                    string filePath = Path.Combine(
                        _webHost.ContentRootPath,
                        "wwwroot",
                        "multipleFileUpload",
                        Path.GetFileName(image.ExistingFilePath)
                    );

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            _context.FileToApis.RemoveRange(images);
            _context.SaveChanges();
        }



    }
}
