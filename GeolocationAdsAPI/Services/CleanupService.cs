using System.IO;

namespace GeolocationAdsAPI.Services
{
    public class CleanupService : BackgroundService
    {
        private readonly string wwwrootPath;

        private readonly TimeSpan fileAgeLimit = TimeSpan.FromDays(5); // Ejemplo: 5 días

        public CleanupService(IWebHostEnvironment env)
        {
            wwwrootPath = $"{env.WebRootPath}\\hls";
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        //var files = Directory.GetFiles(wwwrootPath);

        //        var files = Directory.GetFiles(wwwrootPath, "*.*", SearchOption.AllDirectories);

        //        foreach (var file in files)
        //        {
        //            try
        //            {
        //                //var fileInfo = new FileInfo(file);

        //                var directoryInfo = new DirectoryInfo(directory);

        //                if (DateTime.UtcNow - fileInfo.LastAccessTimeUtc > fileAgeLimit)
        //                {
        //                    fileInfo.Delete();
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                // Log or handle exceptions
        //            }
        //        }

        //        await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Ejecutar una vez al día
        //    }
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var directories = Directory.GetDirectories(wwwrootPath);

                foreach (var directory in directories)
                {
                    try
                    {
                        var directoryInfo = new DirectoryInfo(directory);

                        if (DateTime.UtcNow - directoryInfo.CreationTimeUtc > fileAgeLimit)
                        {
                            // This will delete the directory and all its contents recursively.
                            directoryInfo.Delete(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle exceptions
                        // For example, you might want to log the exception message along with the directory path
                    }
                }

                // Wait for a day or any interval before the next cleanup
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Run once a day
            }
        }
    }
}