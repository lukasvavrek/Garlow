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

namespace Garlow.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/")]
    [ApiController]
    public class MovementsController : ControllerBase
    {
        private readonly IGarlowRepository _garlowRepository;
        private readonly IHubContext<MovementsChartHub> _hubContext;
        
        public MovementsController(
            IGarlowRepository garlowRepository,
            IHubContext<MovementsChartHub> hubContext)
        {
            _garlowRepository = garlowRepository;
            _hubContext = hubContext;
        }

        [HttpGet("{locationId}")]
        public async Task<IActionResult> GetMovements(int locationId)
        {
            var locationFromRepo = await _garlowRepository.GetLocation(locationId);
            if (locationFromRepo == null)
                return BadRequest();

            if (locationFromRepo.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var movements = await _garlowRepository.GetMovements(locationId);
            // TODO: map to simpler dto, transform data

            return Ok(new {
                Counts = movements.TakeLast(20).Select(m => m.Direction).ToArray()
            });


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

            locationFromRepo.Movements.Add(new Movement
            {
                At = DateTime.Now,
                Direction = movement
            });

            if (await _garlowRepository.SaveAll())
            {
                await _hubContext.Clients.All.SendAsync("remote-method", $"{movement}");
                return Ok();
            }

            return BadRequest("Failed to log movement.");
        }
    }
}