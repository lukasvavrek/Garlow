using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Garlow.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Garlow.API.Data
{
    public class CloudinaryPhotoUploader : IPhotoUploader
    {
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        
        private readonly Cloudinary _cloudinary;

        public CloudinaryPhotoUploader(IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;

            var account = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public ImageUploadResult UploadPhoto(IFormFile file, CropSettings cropSettings)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(cropSettings.Width)
                            .Height(cropSettings.Height)
                            .Crop(cropSettings.CropType)
                            .Gravity(cropSettings.Gravity)
                    };
                    return _cloudinary.Upload(uploadParams);
                }
            }

            return null;
        }
        
        // TODO: return enum success/failure
        public DeletionResult DestroyPhoto(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            return _cloudinary.Destroy(deleteParams);
        }
    }
}