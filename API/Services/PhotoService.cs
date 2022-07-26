using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API.Services
{
    public class PhotoService : IPhotoService
    {
        public readonly Cloudinary _Cloudinary;
        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _Cloudinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
         var uploadResult = new ImageUploadResult();
         if(file.Length > 0){

            using var stream = file.OpenReadStream();
            var uploadParm = new ImageUploadParams{
                File = new FileDescription(file.FileName , stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
            };
            uploadResult = await _Cloudinary.UploadAsync(uploadParm);
         }
         return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParm = new DeletionParams(publicId);

            var result = await _Cloudinary.DestroyAsync(deleteParm);
            return result;
        }
    }
}