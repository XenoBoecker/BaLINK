using GameEvents;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISoundManager : MonoBehaviour
{
    [SerializeField] private AudioSystemClip _hoverAudioClip;
    [SerializeField] private AudioSystemClip _clickAudioClip;

    bool _mouseIsOverButton;

    private void Update()
    {
        if (!_mouseIsOverButton)
        {
            if (IsMouseOverUIButton())
            {
                _mouseIsOverButton = true;
                PlayAudioClip(_hoverAudioClip);

            }
            
        } 
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayAudioClip(_clickAudioClip);
            }
            
            if (!IsMouseOverUIButton())
            {
                _mouseIsOverButton = false;
            }
        }
    }

    private void PlayAudioClip(AudioSystemClip clip)
    {
        ObjectEvents.PlayAudio(clip, Vector3.zero);
    }

    bool IsMouseOverUIButton()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
            {
                return true;
            }
        }

        return false;
    }
}
