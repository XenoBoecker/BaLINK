using GameEvents;
using TMPro;
using UnityEngine;

public class VersionControlTextDisplayController : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    private int _versionCounter;

    private void OnEnable()
    {
        ObjectEvents.OnGraphMadeBlinkChange += UpdateVersionText;
    }

    private void OnDisable()
    {
        ObjectEvents.OnGraphMadeBlinkChange -= UpdateVersionText;
    }

    private void UpdateVersionText()
    {
        _versionCounter++;
        _text.text = $"v{_versionCounter} - {GetRandomHash()}";
    }

    private string GetRandomHash()
    {
        string retHash = "";
        int countSinceDash = 0;

        for (int i = 0; i < 12; i++)
        {
            int rand = Random.Range(0, 2);
            if (countSinceDash > 3 * (1 + rand))
            {
                rand = 45;
                countSinceDash = 0;
            } 
            else if (rand == 0)
            {
                rand = Random.Range(48, 57);
                countSinceDash++;
            } 
            else if (rand == 1)
            {
                rand = Random.Range(97, 122);
                countSinceDash++;
            }

            retHash += char.ConvertFromUtf32(rand);
        }

        return retHash;
    }
}
