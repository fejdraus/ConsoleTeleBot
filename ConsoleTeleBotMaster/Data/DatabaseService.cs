using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Services;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;

namespace ConsoleTeleBotMaster.Data;
public class DatabaseService
{
    private readonly IWebHostEnvironment _env;
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public DatabaseService(IDbContextFactory<ApplicationDbContext> dbContextFactory, IWebHostEnvironment env)
    {
        _env = env;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<string> CreateFileCopyAndArchive(List<string> sourceFileNames)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync();
        await context.Database.CloseConnectionAsync();
        var archiveName = "Export_" + string.Join("_", sourceFileNames.Select(Path.GetFileNameWithoutExtension)) + ".zip";
        var targetArchivePath = Path.Combine(_env.WebRootPath, archiveName);
        if (File.Exists(targetArchivePath))
        {
            File.Delete(targetArchivePath);
        }

        List<string> targetFilePaths = new List<string>();

        foreach (var sourceFileName in sourceFileNames)
        {
            var sourceFilePath = Path.Combine(_env.ContentRootPath, sourceFileName);
            var targetFilePath = Path.Combine(_env.WebRootPath, sourceFileName);
        
            if (File.Exists(targetFilePath))
            {
                File.Delete(targetFilePath);
            }
        
            File.Copy(sourceFilePath, targetFilePath);
            targetFilePaths.Add(targetFilePath);
        }

        await using (var archiveStream = File.OpenWrite(targetArchivePath))
        {
            using (var archive = ZipArchive.Create())
            {
                List<MemoryStream> fileDataStreams = new List<MemoryStream>();
                foreach (var targetFilePath in targetFilePaths)
                {
                    var fileName = Path.GetFileName(targetFilePath);
                    var fileData = await File.ReadAllBytesAsync(targetFilePath);
                    var fileDataStream = new MemoryStream(fileData);
                    archive.AddEntry(fileName, fileDataStream);
                    fileDataStreams.Add(fileDataStream);
                }
                archive.SaveTo(archiveStream, CompressionType.Deflate);
                foreach (var fileDataStream in fileDataStreams)
                {
                    await fileDataStream.DisposeAsync();
                }
            }
        }

        foreach (var targetFilePath in targetFilePaths)
        {
            File.Delete(targetFilePath);
        }

        await context.Database.OpenConnectionAsync();
        return targetArchivePath;
    }
    
    public async Task<string> ImportDatabaseAsync(IBrowserFile file)
    {
        var error = "";
        var buffer = new byte[file.Size];
        await file.OpenReadStream().ReadAsync(buffer);
        var tempFilePath = Path.GetTempFileName();
        await File.WriteAllBytesAsync(tempFilePath, buffer);
        try
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite($"Data Source={tempFilePath}");
            await using var tempContext = new ApplicationDbContext(optionsBuilder.Options);
            var tempData = await tempContext.ParsingRules.ToListAsync();
            await using var targetContext = await _dbContextFactory.CreateDbContextAsync();
            var existingData = await targetContext.ParsingRules.ToListAsync();
            targetContext.ParsingRules.RemoveRange(existingData);
            targetContext.ParsingRules.AddRange(tempData);
            await targetContext.SaveChangesAsync();
            await tempContext.Database.CloseConnectionAsync();
        }
        catch (SqliteException)
        {
            error = "Файл не является базой данных SQLite";
        }
        catch (Exception ex) // Общий блок catch для обработки всех исключений
        {
            error = $"Произошла ошибка при импорте базы данных: {ex.Message}";
        }
        finally
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        return error;
    }
}