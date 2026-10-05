using Microsoft.EntityFrameworkCore;
using WebAppShop.Core.Domain;
using WebAppShop.Core.Dto;
using WebAppShop.Core.ServiceInterface;
using WebAppShop.Data;


namespace WebAppShop.ApplicationServices.Services
{


    public class SpaceshipServices : ISpaceshipServices
    {
        private readonly WebAppShopContext _context;

        public SpaceshipServices
            (
                WebAppShopContext context
            )
        {
            _context = context;
        }

        public async Task<Spaceship> Create(SpaceshipDto dto)
        {
            Spaceship domain = new();

            domain.Id = dto.Id;
            domain.Name = dto.Name;
            domain.Classification = dto.Classification;
            domain.BuiltDate = dto.BuiltDate;
            domain.Crew = dto.Crew;
            domain.EnginePower = dto.EnginePower;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            //siia tuleb kood, mis salvestab domain
            //objekti andmebaasi
            //tuleb kasutada repository'd, mis
            //on defineeritud Core projektis
            //konstruktori kaudu tuleb injectida repository

            await _context.Spaceships.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        //siia teha meetod nimega DetailsAsync
        //see ainult p'rib andmed contextist
        public async Task<Spaceship> DetailsAsync(Guid id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Spaceship> Update(SpaceshipDto dto)
        {
            Spaceship spaceship = new();

            spaceship.Id = dto.Id;
            spaceship.Name = dto.Name;
            spaceship.Classification = dto.Classification;
            spaceship.BuiltDate = dto.BuiltDate;
            spaceship.Crew = dto.Crew;
            spaceship.EnginePower = dto.EnginePower;
            spaceship.CreatedAt = dto.CreatedAt;
            spaceship.ModifiedAt = DateTime.Now;

            _context.Spaceships.Update(spaceship);
            await _context.SaveChangesAsync();

            return spaceship;
        }

        public async Task<Spaceship> Delete(Guid id)
        {
            var result = await _context.Spaceships
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Spaceships.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
