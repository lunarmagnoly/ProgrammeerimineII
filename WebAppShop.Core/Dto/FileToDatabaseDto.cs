using System;
using System.Collections.Generic;
using System.Text;

namespace WebAppShop.Core.Dto
{
    public class FileToDatabaseDto
    {
        public Guid Id { get; set; }
        public string? ImageTitle { get; set; }
        public byte[]? ImageData { get; set; }
        public Guid? SpaceshipId { get; set; }
    }
}
