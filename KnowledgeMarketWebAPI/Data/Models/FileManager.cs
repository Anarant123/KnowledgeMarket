namespace KnowledgeMarketWebAPI.Data;

public class FileManager
{
    private readonly string _coursePhotoPath;
    private readonly string _defaultCoursePhotoName;

    public FileManager(IConfiguration configuration)
    {
        _coursePhotoPath = configuration["Path:CoursePhoto"]!;
        _defaultCoursePhotoName = configuration["Path:DefaultCoursePhoto"]!;

        if (!Directory.Exists(_coursePhotoPath)) Directory.CreateDirectory(_coursePhotoPath);
    }

    private static async Task<string> SaveFile(IFormFile? file, string basePath)
    {
        if (file is null) return basePath["wwwroot/".Length..];

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

        await using var fileStream = new FileStream(Path.Combine(basePath, fileName), FileMode.Create);
        await file.CopyToAsync(fileStream);

        return basePath["wwwroot/".Length..] + fileName;
    }

    public async Task<string> SaveCoursePhoto(IFormFile? file)
    {
        if (file is null) return await SaveFile(null, _defaultCoursePhotoName);
        return await SaveFile(file, _coursePhotoPath);
    }

    public bool DeleteAvatar(string avatarFileName)
    {
        // TODO:
        /*var path = Path.Combine(_basePath, avatarFileName);
        if (!File.Exists(path)) return false;

        try
        {
            File.Delete(path);
        }
        catch (Exception)
        {
            return false;
        }

        return true;*/

        throw new NotImplementedException();
    }

    public Task<string?> UpdateAvatar(string? oldAvatarFileName, string newFilePath)
    {
        // TODO:
        /*if (oldAvatarFileName is null) return await AddAvatar(newFilePath);

        var oldAvatarFilePath = Path.Combine(_basePath, oldAvatarFileName);

        try
        {
            await using var oldAvatarFileStream = new FileStream(oldAvatarFilePath, FileMode.Truncate);
            await using var fromFileStream = File.OpenRead(newFilePath);
            await fromFileStream.CopyToAsync(oldAvatarFileStream);
        }
        catch (Exception)
        {
            return null;
        }

        return oldAvatarFileName;*/

        throw new NotImplementedException();
    }
}