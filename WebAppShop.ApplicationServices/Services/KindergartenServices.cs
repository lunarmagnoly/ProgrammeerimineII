using Microsoft.EntityFrameworkCore;
using WebAppShop.Core.Domain;
using WebAppShop.Core.Dto;
using WebAppShop.Core.ServiceInterface;
using WebAppShop.Data;

namespace WebAppShop.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly WebAppShopContext _context;

        public KindergartenServices
            (
                WebAppShopContext context
            )
        {
            _context = context;
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten domain = new();

            domain.Id = dto.Id;
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.TeacherName = dto.TeacherName;
            domain.CreatedAt = DateTime.Now;
            domain.UpdatedAt = null;

            //siia tuleb kood, mis salvestab domain
            //objekti andmebaasi
            //tuleb kasutada repository'd, mis
            //on defineeritud Core projektis
            //konstruktori kaudu tuleb injectida repository

            await _context.Kindergartens.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        //siia teha meetod nimega DetailsAsync
        //see ainult p'rib andmed contextist
        public async Task<Kindergarten> DetailsAsync(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            Kindergarten kindergarten = new();

            kindergarten.Id = dto.Id;
            kindergarten.GroupName = dto.GroupName;
            kindergarten.ChildrenCount = dto.ChildrenCount;
            kindergarten.KindergartenName = dto.KindergartenName;
            kindergarten.TeacherName = dto.TeacherName;
            kindergarten.CreatedAt = dto.CreatedAt;
            kindergarten.UpdatedAt = DateTime.Now;

            _context.Kindergartens.Update(kindergarten);
            await _context.SaveChangesAsync();

            return kindergarten;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Kindergartens.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
