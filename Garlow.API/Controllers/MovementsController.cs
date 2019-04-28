using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Garlow.API.Data;
using Garlow.API.Dtos;
using Garlow.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Garlow.API.HubConfig;
using Newtonsoft.Json;
using AutoMapper;

namespace Garlow.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/")]
    [ApiController]
    public class MovementsController : ControllerBase
    {
        private readonly IGarlowRepository _garlowRepository;
        private readonly IHubContext<MovementsChartHub> _hubContext;
        private readonly IMapper _mapper;
        
        public MovementsController(
            IGarlowRepository garlowRepository,
            IHubContext<MovementsChartHub> hubContext,
            IMapper mapper)
        {
            _garlowRepository = garlowRepository;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpGet("{locationId}")]
        public async Task<IActionResult> GetMovements(int locationId)
        {
            var locationFromRepo = await _garlowRepository.GetLocation(locationId);
            if (locationFromRepo == null)
                return BadRequest();

            if (locationFromRepo.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var movements = (await _garlowRepository.GetMovements(locationId)).Where(m => m.At.Date == DateTime.Today.Date);
            // TODO: map to simpler dto, transform data

            var lastMovements = movements.TakeLast(20).Select(m => new MovementToReturnDto { Direction = m.Direction, At = m.At }).ToArray();
            var lastSum = lastMovements.Sum(m => m.Direction);
            var sumUntilLast = movements.Sum(m => m.Direction) - lastSum;

            return Ok(new GetMovementsDto { SumUntil = sumUntilLast, LastMovements = lastMovements });
                
            // var sums = movements
            //     // .Where(m => m.At.Date == DateTime.Today)
            //     .GroupBy(m => $"{m.At.Hour}:{m.At.Minute}:{m.At.Second < 30}")
            //     // .GroupBy(m => $"{m.At.Hour}:{m.At.Minute}")
            //     .Select(gr => new { Sum = gr.Sum(m => m.Direction)})
            //     .ToArray();

            // var counts = new List<int> { 0 };
            // for (var i = 0; i < sums.Count(); i++)
            // {
            //     counts.Add(sums[i].Sum + counts[i]);
            // }

            // return Ok(new {
            //     Counts = counts
            // });
        }

        [HttpPost("in")]
        [AllowAnonymous]
        public async Task<IActionResult> LogMovementIn([FromBody] MovementCreateVerificationDto verificationDto)
        {
            return await LogMovement(1, verificationDto);
        }

        [HttpPost("out")]
        [AllowAnonymous]
        public async Task<IActionResult> LogMovementOut([FromBody] MovementCreateVerificationDto verificationDto)
        {
            return await LogMovement(-1, verificationDto);
        }

        private async Task<IActionResult> LogMovement(int movement, MovementCreateVerificationDto verificationDto)
        {
            var locationFromRepo = await _garlowRepository.GetLocation(verificationDto.PublicId, verificationDto.SecretKey);
            if (locationFromRepo == null)
                return BadRequest();

            var dbMovement = new Movement
            {
                At = DateTime.Now,
                Direction = movement
            };
            locationFromRepo.Movements.Add(dbMovement);

            if (await _garlowRepository.SaveAll())
            {
                await _hubContext.Clients.All.SendAsync("remote-method", _mapper.Map<MovementToReturnDto>(dbMovement));
                return Ok();
            }

            return BadRequest("Failed to log movement.");
        }
    }
}