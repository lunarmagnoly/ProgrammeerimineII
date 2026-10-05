using WebAppShop.Core.Domain;
using WebAppShop.Core.Dto;


namespace WebAppShop.Core.ServiceInterface
{
    public interface ISpaceshipServices
    {
        Task<Spaceship> Create(SpaceshipDto dto);
        Task<Spaceship> DetailsAsync(Guid id);
        Task<Spaceship> Update(SpaceshipDto dto);
        Task<Spaceship> Delete(Guid id);
    }
}
