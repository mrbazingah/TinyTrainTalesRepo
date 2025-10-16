using UnityEngine;
using TMPro;    
using System.IO;

public class BuildVersionDisplay : MonoBehaviour
{
    public TextMeshProUGUI versionText; // assign your UI Text here

    void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "BuildInfo/BuildVersion.txt");

        if (File.Exists(filePath))
        {
            string version = File.ReadAllText(filePath);
            versionText.text = version;
        }
        else
        {
            versionText.text = "Unknown Build";
            Debug.LogWarning($"BuildVersion.txt not found at {filePath}");
        }
    }
}
