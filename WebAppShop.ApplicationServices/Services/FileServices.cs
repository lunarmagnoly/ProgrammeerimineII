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

    }
}
