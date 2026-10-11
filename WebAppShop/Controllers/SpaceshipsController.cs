using Microsoft.AspNetCore.Mvc;
using WebAppShop.Models.Spaceship;
using WebAppShop.Core.Dto;
using WebAppShop.Core.ServiceInterface;
using WebAppShop.Data;


namespace WebAppShop.Controllers
{
    public class SpaceshipController : Controller
    {
        private readonly ISpaceshipServices _spaceshipService;
        private readonly WebAppShopContext _context;

        private readonly IFileServices _fileServices;

        public SpaceshipController(
            ISpaceshipServices spaceshipService,
            WebAppShopContext context,
            IFileServices fileServices)
        {
            _spaceshipService = spaceshipService;
            _context = context;
            _fileServices = fileServices;
        }


        public IActionResult Index()
        {
            //loome vaheinstantsi domaini ja viewModeli vahel.
            var result = _context.Spaceships
                .Select(x => new SpaceshipIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Classification = x.Classification,
                    BuiltDate = x.BuiltDate,
                    Crew = x.Crew,
                    EnginePower = x.EnginePower
                });

            return View(result);
        }

        //kui kasutaja klikib "Create" nuppu, siis see meetod käivitatakse
        //tagastab kasutajale vormi, kuhu saab sisestada andmed
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //kui oled teinud vormi, siis see meetod käivitatakse
        //saadab andmed serverisse, kus need salvestatakse andmebaasi
        [HttpPost]
        public async Task<IActionResult> Create(SpaceshipCreateViewModel vm)
        {
            //luua vaheinstants, mis sisaldab andmeid, mis on saadud vormist
            //need andmed tuleb edasi saata dto-sse, mis on mõeldud andmebaasi salvestamiseks

            var dto = new SpaceshipDto
            {
                Name = vm.Name,
                Classification = vm.Classification,
                BuiltDate = vm.BuiltDate,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                Files = vm.Files
            };

            //kutsuda teenuse meetodit, mis salvestab andmed andmebaasi
            var result = await _spaceshipService.Create(dto);

            return RedirectToAction(nameof(Index));
        }

        //tuleb teha Details meetod
        //see kutsub v'lja interfacest service meetodi

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            //meetodi kutsumine interfacest
            var spaceship = await _spaceshipService.DetailsAsync(id);

            //veakäsitlus
            //suunab vaatele NotFound, kui andmeid ei ole
            if (spaceship == null)
            {
                return NotFound();
            }

            //tuleb teha viewModel ja see siin välja kutsuda
            //ära map-ida vm ja domain
            var vm = new SpaceshipDetailsViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.FileToApiDtos = _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(x => new FileToApiDto
                {
                    Id = x.Id,
                    ExistingFilePath = x.ExistingFilePath,
                    SpaceshipId = x.SpaceshipId
                })
                .ToList();
            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            var vm = new SpaceshipUpdateViewModel
            {
                Id = spaceship.Id,
                Name = spaceship.Name,
                Classification = spaceship.Classification,
                BuiltDate = spaceship.BuiltDate,
                Crew = spaceship.Crew,
                EnginePower = spaceship.EnginePower,
                CreatedAt = spaceship.CreatedAt,
                ModifiedAt = spaceship.ModifiedAt
            };

            vm.FileToApiDtos = _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(x => new FileToApiDto
                {
                    Id = x.Id,
                    ExistingFilePath = x.ExistingFilePath,
                    SpaceshipId = x.SpaceshipId
                })
                .ToList();

            return View(vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SpaceshipUpdateViewModel vm)
        {
            if (!_context.Spaceships.Any(x => x.Id == vm.Id))
            {
                return NotFound();
            }

            var dto = new SpaceshipDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Classification = vm.Classification,
                Crew = vm.Crew,
                EnginePower = vm.EnginePower,
                BuiltDate = vm.BuiltDate,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files
            };

            await _spaceshipService.Update(dto);

            if (vm.DeleteImageIds != null && vm.DeleteImageIds.Count > 0)
            {
                var validImageIds = _context.FileToApis
                    .Where(x => x.SpaceshipId == vm.Id
                             && vm.DeleteImageIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToList();

                _fileServices.DeleteSelectedFiles(validImageIds);
            }

            return RedirectToAction(nameof(Details), new { id = vm.Id });
        }


        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var spaceship = await _spaceshipService.DetailsAsync(id);

            if (spaceship == null)
            {
                return NotFound();
            }

            var vm = new SpaceshipDeleteViewModel();

            vm.Id = spaceship.Id;
            vm.Name = spaceship.Name;
            vm.Classification = spaceship.Classification;
            vm.BuiltDate = spaceship.BuiltDate;
            vm.Crew = spaceship.Crew;
            vm.EnginePower = spaceship.EnginePower;
            vm.CreatedAt = spaceship.CreatedAt;
            vm.ModifiedAt = spaceship.ModifiedAt;
            vm.FileToApiDtos = _context.FileToApis
                .Where(x => x.SpaceshipId == id)
                .Select(x => new FileToApiDto
                {
                    Id = x.Id,
                    ExistingFilePath = x.ExistingFilePath,
                    SpaceshipId = x.SpaceshipId
                })
                .ToList();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var result = await _spaceshipService.Delete(id);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSelectedImages(Guid spaceshipId, List<Guid> imageIds)
        {
            if (!_context.Spaceships.Any(x => x.Id == spaceshipId))
            {
                return NotFound();
            }

            if (imageIds != null && imageIds.Count > 0)
            {
                var validImageIds = _context.FileToApis
                    .Where(x => x.SpaceshipId == spaceshipId
                             && imageIds.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToList();

                _fileServices.DeleteSelectedFiles(validImageIds);
            }

            return RedirectToAction(nameof(Details), new { id = spaceshipId });
        }

    }
}
