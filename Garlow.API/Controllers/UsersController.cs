using Garlow.API.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Garlow.API.Dtos;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace Garlow.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IGarlowRepository _garlowRepository;
        private readonly IMapper _mapper;

        public UsersController(IGarlowRepository garlowRepository, IMapper mapper)
        {
            _garlowRepository = garlowRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _garlowRepository.GetUsers();
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _garlowRepository.GetUser(userId);
            var userToReturn = _mapper.Map<UserForDetailDto>(user);
            return Ok(userToReturn);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody]UserForUpdateDto userForUpdateDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userFromRepo = await _garlowRepository.GetUser(userId);
            _mapper.Map(userForUpdateDto, userFromRepo);

            if (await _garlowRepository.SaveAll())
                return NoContent();
            
            throw new Exception($"Updating user {userId} failed on save.");
        }
    }
}
