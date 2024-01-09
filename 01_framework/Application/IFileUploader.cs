using Microsoft.AspNetCore.Http;


namespace _01_framework.Application
{
	public interface IFileUploader
	{
        string Upload(IFormFile file,string path);

	}
}
