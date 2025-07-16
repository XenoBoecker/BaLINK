using GameEvents;
using System;
using UnityEngine;

public class UIVersionNumber : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI versionText;

    [SerializeField] private string versionFormat = "Version {0} ({1})";

    [SerializeField] private string versionNumber = "1.0.0";

    [SerializeField] private string buildDate = "2025-06-20"; // Example date, replace with actual build date
    [SerializeField] private bool useCurrentDate = false; // Use current date if true

    [SerializeField] int blinkMinDaysIncrease = 0; // Minimum days to increase on blink
    [SerializeField] int blinkMaxDaysIncrease = 5; // Maximum days to increase on blink

    int majorVersion = 1;
    int minorVersion = 0;
    int patchVersion = 0;

    private void Start()
    {
        if (useCurrentDate)
        {
            buildDate = DateTime.Now.ToString("yyyy-MM-dd"); // Set to current date
        }

        UpdateUI();

        InputEvents.onPlayerBlinked += OnPlayerBlinked;
    }

    private void OnPlayerBlinked()
    {
        IncreaseVersionNumber(2); // Increase patch version on blink

        IncreaseBuildDate(UnityEngine.Random.Range(blinkMinDaysIncrease, blinkMaxDaysIncrease)); // Increase build date by 1 day on blink
    }

    void UpdateUI()
    {
        if (versionText != null) {
            versionText.text = string.Format(versionFormat, versionNumber, buildDate);
        }
        else
        {
            Debug.LogWarning("Version text component is not assigned.", this);
        }
    }

    public void SetVersionNumber(string newVersionNumber)
    {
        versionNumber = newVersionNumber;

        // Parse the version number to update major, minor, and patch versions
        string[] versionParts = newVersionNumber.Split('.');
        if (versionParts.Length == 3)
        {
            if (int.TryParse(versionParts[0], out int major)) majorVersion = major;
            if (int.TryParse(versionParts[1], out int minor)) minorVersion = minor;
            if (int.TryParse(versionParts[2], out int patch)) patchVersion = patch;
        }
        else
        {
            Debug.LogWarning("Invalid version number format. Expected format: Major.Minor.Patch");
        }
        UpdateUI();
    }

    public void SetBuildDate(string newBuildDate)
    {
        buildDate = newBuildDate;
        UpdateUI();
    }

    public void IncreaseVersionNumber(int index)
    {
        switch (index)
        {
            case 0: // Major
                majorVersion++;
                minorVersion = 0;
                patchVersion = 0;
                break;
            case 1: // Minor
                minorVersion++;
                patchVersion = 0;
                break;
            case 2: // Patch
                patchVersion++;
                break;
            default:
                Debug.LogWarning("Invalid version index. Use 0 for Major, 1 for Minor, or 2 for Patch.");
                return;
        }

        versionNumber = $"{majorVersion}.{minorVersion}.{patchVersion}";

        UpdateUI();
    }

    public void IncreaseBuildDate(int daysToAdd)
    {
        if (DateTime.TryParse(buildDate, out DateTime date))
        {
            date = date.AddDays(daysToAdd);
            buildDate = date.ToString("yyyy-MM-dd");
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Invalid build date format. Expected format: yyyy-MM-dd");
        }
    }

    private void OnDestroy()
    {
        InputEvents.onPlayerBlinked -= OnPlayerBlinked;
    }
}
