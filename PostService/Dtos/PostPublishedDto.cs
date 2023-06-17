using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using System.IO;

namespace PostService.Dtos
{
    public class PostPublishedDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public byte[] ImageFile { get; set; }
        public string UserId { get; set; }
        public string Event { get; set; }

        public PostPublishedDto()
        {
            
        }

        public PostPublishedDto(PostReadDto postReadDto)
        {
            Id = postReadDto.Id.ToString();
            Title = postReadDto.Title;

            /*using (var stream = postReadDto.ImageFile.OpenReadStream())
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                ImageFile = memoryStream.ToArray();
            }*/


            /*using (var memoryStream = new MemoryStream())
            {
                postReadDto.ImageFile.CopyTo(memoryStream);

                ImageFile = memoryStream.ToArray();
            }*/


            /*using (var stream = postReadDto.ImageFile.OpenReadStream())
            {
                byte[] fileBytes = new byte[postReadDto.ImageFile.Length];
                stream.Read(fileBytes, 0, (int)postReadDto.ImageFile.Length);
                ImageFile = fileBytes;
            }*/


            /*using var memoryStream = new MemoryStream();
            postReadDto.ImageFile.CopyToAsync(memoryStream);
            ImageFile = memoryStream.ToArray();*/


            /*ImageFile = GetBytesAsync(postReadDto.ImageFile);*/


            /*using (var memoryStream = new MemoryStream())
            {
                postReadDto.ImageFile.CopyTo(memoryStream);
                ImageFile = memoryStream.ToArray();
            }*/

            /*if (postReadDto.ImageFile != null && postReadDto.ImageFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    var buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = postReadDto.ImageFile.OpenReadStream().Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, bytesRead);
                    }
                    ImageFile = memoryStream.ToArray();
                }
            }*/
            ImageFile = ConvertFormFileToBytes(postReadDto.ImageFile);

            UserId = postReadDto.UserId.ToString();
        }

        public byte[] ConvertFormFileToBytes(IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
