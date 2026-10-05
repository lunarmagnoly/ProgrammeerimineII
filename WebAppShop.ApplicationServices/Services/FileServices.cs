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
            //kindlasti peab ankeedil olema üks fail
            if (dto.Files != null && dto.Files.Count > 0)
            {
                //kui ei ole wwwroot-s multipleFileUpload directoryt
                if (!Directory.Exists(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\"))
                {
                    //, siis tee directory wwwrooti alla
                    Directory.CreateDirectory(_webHost.ContentRootPath + "\\wwwroot\\multipleFileUpload\\");
                }

                foreach (var file in dto.Files)
                {
                    //meil on vaja teha muutuja nimega uploadsFolder.
                    //sinna muutuja taha on vaja Path kombineerida
                    string uploadsFolder = Path.Combine(_webHost.ContentRootPath, "wwwroot", "multipleFileUpload");
                    //igale failile unikaalne Guid selle nime ette
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.Name;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);

                        //domaini teha FileToApi
                        //FileToApi
                    }
                }
            }
        }
    }
}
