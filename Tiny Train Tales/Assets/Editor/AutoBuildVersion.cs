using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System;
using System.IO;

public class AutoBuildVersion : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        // Folder path inside StreamingAssets
        string folderPath = "Assets/StreamingAssets/BuildInfo";
        string filePath = Path.Combine(folderPath, "BuildVersion.txt");

        // Ensure the folder exists
        Directory.CreateDirectory(folderPath);

        // Date part for version
        string datePart = DateTime.Now.ToString("yyyy.MM.dd");
        int buildCount = 1;

        // Check last build number
        if (File.Exists(filePath))
        {
            string lastVersion = File.ReadAllText(filePath);
            if (lastVersion.StartsWith($"Build {datePart}"))
            {
                string[] parts = lastVersion.Split('.');
                if (int.TryParse(parts[^1], out int lastBuild))
                    buildCount = lastBuild + 1;
            }
        }

        // New build version
        string newVersion = $"Build {datePart}.{buildCount:D3}";

        // Write version file
        File.WriteAllText(filePath, newVersion);
    }
}
