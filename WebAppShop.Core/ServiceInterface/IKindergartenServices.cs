using System;
using System.Collections.Generic;
using System.Text;
using WebAppShop.Core.Domain;
using WebAppShop.Core.Dto;

namespace WebAppShop.Core.ServiceInterface
{
    public interface IKindergartenServices
    {
        Task<Kindergarten> Create(KindergartenDto dto);
        Task<Kindergarten> DetailsAsync(Guid id);
        Task<Kindergarten> Update(KindergartenDto dto);
        Task<Kindergarten> Delete(Guid id);
    }
}
