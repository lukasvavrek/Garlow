using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Garlow.API.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string PublicId { get; set; }
        public string SecretKey { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public string PhotoUrl { get; set; }
        public string PhotoPublicId { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Movement> Movements { get; set; }
    }
}