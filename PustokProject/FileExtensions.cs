using System;

public static class FileExtensions
{
	public const string ImageFolderPath = "wwwroot/bookimages";
	public const string UsersFolderPath = "wwwroot/userimages";



	public async static Task<string> SaveToRootWithUniqueNameAsync(this IFormFile file,bool isUserImage = false)
	{

		var rootPath = Directory.GetCurrentDirectory();
		string imagePath = null;
		if (isUserImage)
		{
			imagePath = Path.Combine(rootPath, UsersFolderPath);
		}
		else
		{
			imagePath = Path.Combine(rootPath, ImageFolderPath);
		}

		var fileName = Guid.NewGuid().ToString()+file.FileName;

		var imageFile = File.Create(Path.Combine(imagePath, fileName));

		await file.CopyToAsync(imageFile);

		imageFile.Close();
		return fileName;
	}
	
}
