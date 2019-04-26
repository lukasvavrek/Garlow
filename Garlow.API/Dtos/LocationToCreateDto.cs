using Microsoft.AspNetCore.Http;

namespace Garlow.API.Dtos
{
    public class LocationToCreateDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public IFormFile Photo { get; set; }
        public string PhotoUrl { get; set; }
        public string PhotoPublicId { get; set; }
    }
}