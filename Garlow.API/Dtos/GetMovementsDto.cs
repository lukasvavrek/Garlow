using System.Collections.Generic;

namespace Garlow.API.Dtos
{
    public class GetMovementsDto
    {
        public int SumUntil { get; set; }
        public IEnumerable<MovementToReturnDto> LastMovements { get; set; }
    }
}