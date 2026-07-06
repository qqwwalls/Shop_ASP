using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Shop.Api.Interfaces;

public interface IImageService
{
    Task<string> SaveFileAsync(IFormFile file, string folderName);
}
