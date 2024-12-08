using System;
using System.IO;
using System.Threading;

class SyncApp
{
    private readonly string sourceDir;
    private readonly string targetDir;
    private readonly string logFile;
    private readonly int intervalSeconds;

    public SyncApp(string source, string target, string logPath, int interval)
    {
        sourceDir = source;
        targetDir = target;
        logFile = logPath;
        intervalSeconds = interval;
    }

    public void Run()
    {
        while (true)
        {
            try
            {
                PerformSync();
            }
            catch (Exception ex)
            {
                WriteLog($"Error during synchronization: {ex.Message}");
            }

            Thread.Sleep(intervalSeconds * 1000);
        }
    }

    private void PerformSync()
    {
        WriteLog("Synchronization started.");

        // Ensure target matches source: Add or update files
        foreach (var entry in Directory.EnumerateFileSystemEntries(sourceDir, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(sourceDir, entry);
            var destPath = Path.Combine(targetDir, relativePath);

            if (Directory.Exists(entry))
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                    WriteLog($"Directory created: {destPath}");
                }
            }
            else if (File.Exists(entry))
            {
                if (!File.Exists(destPath) || File.GetLastWriteTimeUtc(destPath).Equals(File.GetLastWriteTimeUtc(entry)))
                {
                    File.Copy(entry, destPath, true);
                    WriteLog($"File copied/updated: {entry} -> {destPath}");
                }
            }
        }


        foreach (var entry in Directory.EnumerateFileSystemEntries(targetDir, "*", SearchOption.AllDirectories))
        {
            var relativePath = Path.GetRelativePath(targetDir, entry);
            var sourcePath = Path.Combine(sourceDir, relativePath);

            if (!File.Exists(sourcePath) && !Directory.Exists(sourcePath))
            {
                if (File.Exists(entry))
                {
                    File.Delete(entry);
                    WriteLog($"File removed: {entry}");
                }
                else if (Directory.Exists(entry))
                {
                    Directory.Delete(entry, true);
                    WriteLog($"Directory removed: {entry}");
                }
            }
        }

        WriteLog("Synchronization completed.");
    }

    private void WriteLog(string message)
    {
        var logEntry = $"[{DateTime.Now}] {message}";
        Console.WriteLine(logEntry);
        File.AppendAllText(logFile, logEntry + Environment.NewLine);
    }

    public static void Main(string[] args)
    {
        // Hardcoded paths for testing, please change this part according to your folder paths
        var source = @"C:\Users\humay\SourceFolder";  
        var target = @"C:\Users\humay\ReplicaFolder"; 
        var logPath = @"C:\Users\humay\sync.log";   

        var interval = 10; // Sync interval in seconds

        if (!Directory.Exists(source))
        {
            Console.WriteLine($"Source folder '{source}' does not exist.");
            return;
        }

        var sync = new SyncApp(source, target, logPath, interval);
        sync.Run();
    }
}

