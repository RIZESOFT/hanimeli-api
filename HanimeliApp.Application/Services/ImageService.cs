using HanimeliApp.Application.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanimeliApp.Application.Services
{
    public class ImageService
    {
        private readonly AzureStorageHelper _azureStorageHelper;

        public ImageService(AzureStorageHelper azureStorageHelper)
        {
            _azureStorageHelper = azureStorageHelper;
        }

        public async Task<string> UploadImageAsync(IFormFile imageFile, string containerName)
        {
            if (imageFile == null)
                throw new Exception("Image is required");

            if (imageFile.Length > 5 * 1024 * 1024)
                throw new Exception("Image size must be less than 5MB");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            await using var stream = imageFile.OpenReadStream();
            var contentType = imageFile.ContentType;
            await _azureStorageHelper.UploadFileAsync(containerName, fileName, stream, contentType);

            return await _azureStorageHelper.GetFilePathAsync(containerName, fileName);
        }

    }
}
