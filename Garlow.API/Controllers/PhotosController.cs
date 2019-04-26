using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Garlow.API.Data;
using Garlow.API.Dtos;
using Garlow.API.Helpers;
using Garlow.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Garlow.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : ControllerBase
    {
        private const string GetPhotoRouteName = "GetPhotoRoute";

        private readonly IGarlowRepository _garlowRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoUploader _photoUploader;

        public PhotosController(
            IGarlowRepository garlowRepository, 
            IMapper mapper,
            IPhotoUploader photoUploader
            )
        {
            _garlowRepository = garlowRepository;
            _mapper = mapper;
            _photoUploader = photoUploader;
        }

        [HttpGet("{photoId}", Name = GetPhotoRouteName)]
        public async Task<IActionResult> GetPhoto(int userId, int photoId)
        {
            var photoFromRepo = await _garlowRepository.GetPhoto(photoId);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _garlowRepository.GetUser(userId);
            var file = photoForCreationDto.File;

            // photo uploading
            var uploadResult = _photoUploader.UploadPhoto(file, new CropSettings {
                Width = 500,
                Height = 500,
                CropType = "fill",
                Gravity = "face"
            });

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);

            if (await _garlowRepository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute(GetPhotoRouteName, new { photoId = photo.Id }, photoToReturn);
            }

            return BadRequest("Could not add the photo.");
        }

        [HttpPost("{photoId}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int photoId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _garlowRepository.GetUser(userId);
            if (!user.Photos.Any(p => p.Id == photoId))
                return Unauthorized();

            var photoFromRepo = await _garlowRepository.GetPhoto(photoId);
            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo.");
            
            var currentMainPhoto = await _garlowRepository.GetMainPhotoForUser(userId);

            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            if (await _garlowRepository.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main.");
        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(int userId, int photoId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _garlowRepository.GetUser(userId);
            if (!user.Photos.Any(p => p.Id == photoId))
                return Unauthorized();

            var photoFromRepo = await _garlowRepository.GetPhoto(photoId);
            if (photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo.");

            if (photoFromRepo.PublicId != null)
            {
                var result = _photoUploader.DestroyPhoto(photoFromRepo.PublicId);
                
                if (result.Result == "ok")
                    _garlowRepository.Delete(photoFromRepo);
            }

            if (photoFromRepo.PublicId == null)
                _garlowRepository.Delete(photoFromRepo);

            if (await _garlowRepository.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo.");
        }
    }
}