

using _01_framework.Application;

namespace ServiceHost
{
    public class FileUploader : IFileUploader
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploader(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string Upload(IFormFile file, string path)
        {
            if (file is null || file.Length == 0)
                return "";

            var directoryPath =$"{_environment.WebRootPath}\\ UploaderFile \\{ path}";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            Directory.CreateDirectory(directoryPath);

            string fileName = $"{DateTime.Now.ToFileTime()}--{file.FileName}";
            string filePath = $"{directoryPath}\\ {fileName}";

             using var output = File.Create(filePath);
            file.CopyTo(output);

          

            return $"{path}\\{fileName}";
        }
    }
}
