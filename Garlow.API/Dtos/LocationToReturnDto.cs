namespace Garlow.API.Dtos
{
    public class LocationToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PublicId { get; set; }
        public string SecretKey { get; set; }

        public string PhotoUrl { get; set; }
    }
}