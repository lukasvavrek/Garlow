using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Garlow.API.Data
{
    public interface IPhotoUploader
    {
        // TODO: change image upload result to something custom to remove dependency on cloudinary
        ImageUploadResult UploadPhoto(IFormFile file, CropSettings cropSettings);
        DeletionResult DestroyPhoto(string publicId);
    }
}