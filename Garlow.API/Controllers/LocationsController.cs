using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Garlow.API.Data;
using Garlow.API.Dtos;
using Garlow.API.Helpers;
using Garlow.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Garlow.API.Controllers
{
    [Authorize]
    [Route("api/[controller]/")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private const string GetLocationRouteName = "GetLocationRouteName";

        private readonly IGarlowRepository _garlowRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoUploader _photoUploader;

        public LocationsController(
            IGarlowRepository garlowRepository, 
            IMapper mapper,
            IPhotoUploader photoUploader
        )
        {
            _garlowRepository = garlowRepository;
            _mapper = mapper;
            _photoUploader = photoUploader;
        }

        [HttpGet("foruser/{userId}")]
        public async Task<IActionResult> LocationForUser(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var locations = await _garlowRepository.GetLocationsForUser(userId);
            var locationsToReturn = _mapper.Map<IEnumerable<LocationToReturnDto>>(locations);
            return Ok(locationsToReturn);
        }

        [HttpGet("{locationId}", Name=GetLocationRouteName)]
        public async Task<IActionResult> GetLocation(int locationId)
        {
            var location = await _garlowRepository.GetLocation(locationId);

            if (location.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var locationToReturn = _mapper.Map<LocationToReturnDto>(location);
            return Ok(locationToReturn);
        }

        [HttpDelete("{locationId}")]
        public async Task<IActionResult> DeleteLocation(int locationId)
        {
            var location = await _garlowRepository.GetLocation(locationId);

            if (location.UserId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (location.PublicId != null)
            {
                var result = _photoUploader.DestroyPhoto(location.PhotoPublicId);
                
                if (result.Result == "ok")
                    _garlowRepository.Delete(location);
            }

            if (location.PublicId == null)
                _garlowRepository.Delete(location);

            if (await _garlowRepository.SaveAll())
                return Ok();

            return BadRequest();
        }

        [HttpPost("foruser/{userId}")]
        public async Task<IActionResult> CreateLocationForUser(int userId, [FromForm] LocationToCreateDto locationToCreate)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _garlowRepository.GetUser(userId);

            // 1 - store photo in cloudly
            // photo uploading
            var uploadResult = _photoUploader.UploadPhoto(locationToCreate.Photo, new CropSettings {
                Width = 500,
                Height = 500,
                CropType = "fill",
                Gravity = "center" // ?
            });
            // 2 - store photo in database
            locationToCreate.PhotoUrl = uploadResult.Uri.ToString();
            locationToCreate.PhotoPublicId = uploadResult.PublicId;
            // 3 - store location in database
            var location = _mapper.Map<Location>(locationToCreate);
            location.PublicId = Guid.NewGuid().ToString();
            location.SecretKey = SecretKeyGenerator.New();
            userFromRepo.Locations.Add(location);

            if (await _garlowRepository.SaveAll())
            {
                // 4 - return created at route
                var locationToReturn = _mapper.Map<LocationToReturnDto>(location);
                return CreatedAtRoute(GetLocationRouteName, new { locationId = location.Id }, locationToReturn);
            }

            return BadRequest("Could not add the location.");
        }
    }
}