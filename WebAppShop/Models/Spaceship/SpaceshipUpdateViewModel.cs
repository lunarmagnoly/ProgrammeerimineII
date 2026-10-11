using Microsoft.AspNetCore.Http;
using WebAppShop.Core.Dto;

namespace WebAppShop.Models.Spaceship
{
    public class SpaceshipUpdateViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Classification { get; set; } = string.Empty;
        public DateTime? BuiltDate { get; set; }
        public int? Crew { get; set; }
        public int? EnginePower { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        // Uute piltide lisamine
        public List<IFormFile> Files { get; set; } = new();

        // Valitud piltide kustutamine
        public List<Guid> DeleteImageIds { get; set; } = new();


        public List<FileToApiDto> FileToApiDtos { get; set; } = new();

    }
}
