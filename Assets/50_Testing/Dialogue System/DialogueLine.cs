using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(1, 6)]
    [SerializeField] private string _text;
    [SerializeField] private AudioClip _clip;

    public string Text => _text;
    public AudioClip Clip => _clip;
}
