using System;

namespace Garlow.API.Models
{
    public class Movement
    {
        public int Id { get; set; }
        public DateTime At { get; set; }
        public int Direction { get; set; } // 1 in ; -1 out
        
        public int LocationId { get; set; }
        public Location Location { get; set; }
    }
}